# ============================================================
# COMPILE DIRECTLY WITH CSC - SELECTED REFERENCES ONLY
# ============================================================

try:
    dotnet_info = subprocess.check_output(
        ["dotnet", "--info"],
        text=True,
        env=env
    )

    # 1. Získání základní cesty (Base Path) k SDK
    base_path = next(line.split(": ", 1)[1].strip() for line in dotnet_info.splitlines() if "Base Path" in line)
    
    # Odvození cesty k runtime balíčku Microsoft.NETCore.App
    # (Často bývá jednodušší mířit přímo do sdíleného frameworku)
    dotnet_root = os.path.dirname(os.path.dirname(base_path))
    
    # TODO: Zde zítra dynamicky doplňte přesnou složku verze .NET 10 (např. "10.0.0")
    # Podle toho, jak máte nakonfigurovaný server.
    reference_path = os.path.join(dotnet_root, "shared", "Microsoft.NETCore.App", "10.0.0")

    # 2. SEZNAM VYBRANÝCH REFERENCÍ (Selected References)
    selected_dlls = [
        "System.Runtime.dll",
        "System.Console.dll",
        "System.Collections.dll",
        "System.Collections.Concurrent.dll",
        "System.Linq.dll",
        "System.Text.Json.dll",
        "netstandard.dll"
    ]

    # 3. Sestavení plných cest a parametrů -r: pro csc
    references = [
        f"-r:{os.path.join(reference_path, dll)}"
        for dll in selected_dlls
    ]

    # 4. Samotná kompilace pomocí csc
    csc_cmd = [
        "csc", 
        "solution.cs", 
        "runner.cs", 
        "-out:program.exe",
        *references
    ]
    
    success, report = run_cmd(csc_cmd, env)
    
    # ... následné ošetření chyb a spuštění vygenerovaného program.exe
