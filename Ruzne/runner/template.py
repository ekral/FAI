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
        
        env["DOTNET_gcServer"] = "0"
        env["DOTNET_GCRegions"] = "0"
        
        env["DOTNET_GCHeapCount"] = "1"
        env["DOTNET_GCLatencyMode"] = "1"
        env["DOTNET_EnableWriteXorExecute"] = "0"

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

        get_path = lambda p: p / "ref" / f"net{major_version}"

        ref_directories = filter(lambda p: p.is_dir() and get_path(p).is_dir(), ref_pack_root.iterdir())
        ref_directories_sorted = sorted(ref_directories, key=lambda p: [int(x) for x in p.name.split('.')], reverse=True)

        if len(ref_directories_sorted) == 0:
            print_error(
                f"Reference assemblies for net{major_version} not found"
            )
            return
        
        ref_directory = ref_directories_sorted[0]

        runtimeconfig = {
            "runtimeOptions": {
                "tfm": f"net{major_version}",
                "framework": {
                    "name": "Microsoft.NETCore.App",
                    "version": ref_directory.name
                }
            }
        }

        target_directory = get_path(ref_directory)

        with open("runner.runtimeconfig.json", "w", encoding="utf-8") as f:
            json.dump(runtimeconfig, f, ensure_ascii=False, indent=2)

        get_option = lambda d: f"/reference:{target_directory / d}"

        success, report = run_cmd(
            [
                "dotnet",
                "exec",
                csc_path,
                "/nologo",
                "/target:exe",
                "/out:runner.dll",
                "/nostdlib+",
                "solution.cs",
                "runner.cs",
                get_option("System.Console.dll"),
                get_option("System.Runtime.dll"),
                get_option("System.Collections.dll"),
                get_option("System.Linq.dll"),
            ],
            env,
            30
        )
        
        if not success:
            print_error(report)
            return
        
        # ========================================================
        # RUN
        # ========================================================

        success, report = run_cmd(
            [
                "dotnet",
                "runner.dll",
            ],
            env
        )
        
        if not success:
            print_error(report)
            return

        if not report:
            print_error("The runner did not return any grading output.")
            return

        try:
            parsed_json = json.loads(report)
            
            print(json.dumps(parsed_json, ensure_ascii=False))

        except json.JSONDecodeError:
            print_error(f"The runner returned invalid JSON grading output: {report}")
            return

    except Exception as e:
        print_error(f"Internal test script error: {str(e)}")
        return

if __name__ == "__main__":
    main()
