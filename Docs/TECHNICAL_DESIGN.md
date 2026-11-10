# Technical Design

No gameplay architecture is settled. Inspect again after the first systems exist.

## Settled facts

- UI: uGUI chosen; no Canvas in a scene yet. Package `com.unity.ugui`. Rule: `.cursor/rules/unity-ugui.mdc`.
- VCS: Git + `.gitignore`. Remote: GitHub (`https://github.com/IndiePH/unity-2d-roguelite.git`).
- Tasks: Trello board https://trello.com/b/wfh43UJ3/2d-roguelite (`To-Do` / `In-Progress` / `Done`). Not a second design docs store.
- Docs: in-repo under `Docs/` only. Collab rules: [README.md](README.md).
- Input: new Input System (`activeInputHandler` 1). Package `com.unity.inputsystem` 1.20.1. Asset: `Assets/Settings/InputSystem_Actions.inputactions`.

## Absent

- Project scripts, `*.asmdef`, `Assets/Scripts`, `Assets/Tests`, EditMode assembly, runtime system owners.

## Active children

None yet. Link decisions and owning specs here when they exist.
