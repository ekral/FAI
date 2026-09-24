import os
import subprocess
import json
from pathlib import Path

def run_cmd(cmd, env, timeout=15):
    try:
        p = subprocess.run(
            cmd,
            stdout=subprocess.PIPE,
            stderr=subprocess.PIPE,
            text=True,
            timeout=timeout,
            env=env
        )

        if p.returncode == 0:
            return True, p.stdout.strip()
        else:
            errors = []
            if p.stderr and p.stderr.strip():
                errors.append(p.stderr.strip())
            if p.stdout and p.stdout.strip():
                errors.append(p.stdout.strip())

            errout = "\n".join(errors) if errors else "The process failed with no output"
            return False, errout

    except subprocess.TimeoutExpired as e:
        stdout = e.stdout or ""
        stderr = e.stderr or ""

        if isinstance(stdout, bytes):
            stdout = stdout.decode("utf-8", errors="replace")
        if isinstance(stderr, bytes):
            stderr = stderr.decode("utf-8", errors="replace")

        timeout_details = [f"Process timed out after {timeout} seconds."]
        if stderr.strip():
            timeout_details.append(f"Stderr:\n{stderr.strip()}")
        if stdout.strip():
            timeout_details.append(f"Stdout:\n{stdout.strip()}")

        return False, "\n".join(timeout_details)

def print_error(message):
    error_result = {
        "fraction": 0.0,
        "epiloguehtml": f"<pre>{message}</pre>" 
    }

    print(json.dumps(error_result, ensure_ascii=False))

def main():

    try:
        # ============================================================
        # STUDENT CODE
        # ============================================================

        student_code = """{{ STUDENT_ANSWER | e('py') }}"""

        with open("solution.cs", "w", encoding="utf-8") as f:
            f.write(student_code)


        # ============================================================
        # TEST CODE
        # ============================================================

        testcases_raw = """{{ TESTCASES | json_encode | e('py') }}"""
        testcases = json.loads(testcases_raw)

        if testcases and len(testcases) > 0:
            prvni_testcase = testcases[0]  # Získání prvního testcase (Test case 1)
            runner_code = prvni_testcase.get("testcode", "")

            if not runner_code.strip():
                print_error("The Test Case does not contain testcode.");
                return

            with open("runner.cs", "w", encoding="utf-8") as f:
                f.write(runner_code)
        else:
            print_error("No test cases provided.");
            return

        # ============================================================
        # ENVIRONMENT
        # ============================================================

        env = os.environ.copy()

        env["DOTNET_NOLOGO"] = "1"
        env["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1"
        env["DOTNET_SKIP_FIRST_TIME_EXPERIENCE"] = "1"
        env["DOTNET_PRINT_TELEMETRY_MESSAGE"] = "false"
        env["MSBuildEnableWorkloadResolver"] = "false"

        common = [
            "/p:MSBuildEnableWorkloadResolver=false",
            "/p:BuildInParallel=false",
            "/p:UseSharedCompilation=false"
        ]


        # ============================================================
        # COMPILE DIRECTLY WITH CSC - NO RESTORE
        # ============================================================

        success, report = run_cmd(
            [
                "dotnet",
                "--info"
            ],
            env
        )
        
        if not success:
            print_error(report)
            return
  
        base_path_string = next(
            line.split(":", 1)[1].strip()
            for line in report.splitlines()
            if "Base Path:" in line
        )

        base_path = Path(base_path_string)

        version = base_path.name

        major_version = ".".join(version.split(".")[:2])

        csc_path = base_path / "Roslyn" / "bincore" / "csc.dll"

        dotnet_root = base_path.resolve().parent.parent

        ref_pack_root = dotnet_root / "packs" / "Microsoft.NETCore.App.Ref"

        target_framework = f"net{major_version}"

        get_target_path = lambda p: p / "ref" / target_framework

        ref_directories = filter(lambda p: p.is_dir() and get_target_path(p).is_dir(), ref_pack_root.iterdir())
        ref_directories_sorted = sorted(ref_directories, key=lambda p: [int(x) for x in p.name.split('.')], reverse=True)

        if len(ref_directories_sorted) == 0:
            print_error(
                f"Reference assemblies for {target_framework} not found"
            )
            return
        
        ref_directory = ref_directories_sorted[0]

        target_directory = get_target_path(ref_directory)
   
        references = [
            target_directory / "System.Console.dll",
            target_directory / "System.Runtime.dll",
            target_directory / "System.Collections.dll",
            target_directory / "System.Linq.dll"
        ]

        compile_cmd = [
            "dotnet",
            "exec",
            csc_path,

            "/nologo",
            "/target:exe",
            "/out:runner.dll",

            "/nostdlib+",

            "solution.cs",
            "runner.cs",
        ]

        compile_cmd.extend(
            f"/reference:{reference}"
            for reference in references
        )

        compile_result = subprocess.run(
            compile_cmd,
            capture_output=True,
            text=True,
            env=env,
            timeout=30
        )

        if compile_result.returncode != 0:
            output = "\n".join(
                x for x in [
                    compile_result.stdout.strip(),
                    compile_result.stderr.strip()
                ]
                if x
            )

            error_result = {
                "fraction": 0.0,
                "epiloguehtml": (
                    "Compilation error:\n<pre>"
                    + output
                    + "</pre>"
                )
            }

            print(json.dumps(error_result, ensure_ascii=False))
            return

        # ========================================================
        # RUN
        # ========================================================

        run_result = subprocess.run(
            ["dotnet", "runner.dll"],
            capture_output=True,
            text=True,
            env=env,
            timeout=15
        )

        if run_result.returncode != 0:
            errors = []

            if run_result.stderr.strip():
                errors.append(run_result.stderr.strip())

            if run_result.stdout.strip():
                errors.append(run_result.stdout.strip())

            error_result = {
                "fraction": 0.0,
                "epiloguehtml": (
                    "Runtime error:\n<pre>"
                    + "\n".join(errors)
                    + "</pre>"
                )
            }

            print(json.dumps(error_result, ensure_ascii=False))
            return

        report = run_result.stdout.strip()

        if not report:
            error_result = {
                "fraction": 0.0,
                "epiloguehtml": (
                    "The program started but returned no grading output."
                )
            }

            print(json.dumps(error_result, ensure_ascii=False))
            return

        try:
            parsed_json = json.loads(report)
            print(json.dumps(parsed_json, ensure_ascii=False))

        except json.JSONDecodeError:
            error_result = {
                "fraction": 0.0,
                "epiloguehtml": (
                    "The program returned invalid JSON "
                    "grading output:\n<pre>"
                    + report
                    + "</pre>"
                )
            }

            print(json.dumps(error_result, ensure_ascii=False))
            return

        except Exception as e:
            print_error(f"Unexpected error while parsing grading output: {str(e)}")
            return

    except Exception as e:
        print_error(f"Internal test script error: {str(e)}")
        return


if __name__ == "__main__":
    main()

