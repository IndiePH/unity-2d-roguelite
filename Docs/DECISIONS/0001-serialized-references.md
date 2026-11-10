Status: accepted
Owner: Vincent Veran
Last updated: 2026-11-10

# Serialized references

Known components and assets are assigned in the Inspector with `[SerializeField]`.

Rejected alternative: discover them at startup with `GetComponent` or a scene search.

Why: the reference is already on the prefab, so init code that searches for it is setup that can be authored once. An object created during play is the exception, because nothing can assign it ahead of time.

Rule: `.cursor/rules/unity-serialized-references.mdc`.
