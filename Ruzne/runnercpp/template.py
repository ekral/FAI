# V Moodle uloze nastavit timeout na 15 sekund, memmory limit na 0 (neomezeno)

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
        # ENVIRONMENT
        # ============================================================

        env = os.environ.copy()
        
        # ============================================================
        # COMPILER VERSION PRETEST
        # ============================================================

        #success, report = run_cmd(
        #    [
        #        "g++",
        #        "--version"
        #    ],
        #    env
        #)
        #
        #print_error(report)
        #return
        
        # ============================================================
        # STUDENT CODE
        # ============================================================

        student_code = """{{ STUDENT_ANSWER | e('py') }}"""

        with open("solution.h", "w", encoding="utf-8") as f:
            f.write(student_code)

        # ============================================================
        # TEST CODE
        # ============================================================

        testcases_raw = """{{ TESTCASES | json_encode | e('py') }}"""
        testcases = json.loads(testcases_raw)

        if testcases and len(testcases) > 0:
            prvni_testcase = testcases[0]
            runner_code = prvni_testcase.get("testcode", "")

            if not runner_code.strip():
                print_error("The Test Case does not contain testcode.");
                return

            with open("main.cpp", "w", encoding="utf-8") as f:
                f.write(runner_code)
        else:
            print_error("No test cases provided.");
            return

        # ============================================================
        # COMPILE DIRECTLY WITH GCC
        # ============================================================

        success, report = run_cmd(
            [
                "g++",
                "-std=c++23",
                "-o",
                "runner",
                "main.cpp"
            ],
            env
        )
        
        if not success:
            print_error(report)
            return
        
        # ========================================================
        # RUN
        # ========================================================

        success, report = run_cmd(
            [
                "./runner",
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
