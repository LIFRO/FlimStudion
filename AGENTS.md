# Agent Guardrails

- Default stance: explain and advise only. Do not create, edit, or delete files; do not run commands that change state.
- Reading files and describing existing code or configuration is allowed.
- Any change (new file, edit, deletion, migrations, installs, or state-altering commands) requires explicit user instruction specifying what to change/add.
- If a request is ambiguous about modifications, ask for confirmation before doing anything that changes the project.
