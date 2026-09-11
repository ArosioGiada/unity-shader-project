# Unity Shader & Environment Design - Horror Island

A horror-themed reimagining of a provided Unity environment, built around a custom
dissolve shader, atmospheric lighting redesign, and gameplay-driven special effects.

## Concept

An isolated island in the middle of the ocean, populated by spiders, wrapped in a cold,
dark night. The environment is deliberately underlit, relying on weak moonlight, warm
lanterns, and a strange red glow suggesting something unnatural beneath the ground.
Opening a chest grants the player the power to burn the spiders — expressed visually
through a custom dissolve shader.

## What this demonstrates

- **Shader programming** — a custom dissolve shader built in Unity Shader Graph, using
  procedural noise (Simple Noise node), object-space height sampling, alpha clipping,
  and an emissive edge highlight for a fire-like burn effect
- **Environment and lighting design** — dual-tone lighting (cold moonlight vs. an
  unnatural red glow), skybox and water integration, and scene composition for
  atmosphere and narrative framing
- **Gameplay scripting (C#)** — animation state management, trigger-based interactions,
  and fixing real bugs along the way (shared material instances causing all spiders to
  dissolve at once; using `Invoke()` to sync VFX timing with animation length)
- **Systems thinking** — the dissolve shader is never shown in isolation; it only
  activates as the output of a full gameplay pipeline (chest → power → spider →
  animation → dissolve)

## Key technical highlights

**Dissolve shader** — noise-driven alpha clipping read from object space (not world
space), so the same material works correctly on every spider regardless of position,
rotation, or scale. A second, offset threshold isolates a thin edge strip, colored with
an HDR emissive value, to make the effect read as fire rather than a simple fade.

**Burn animation** — the shader itself is static; the burn is driven by animating a
`CutoffHeight` parameter over time via `Mathf.Lerp()` in C#, triggered by player
proximity after the chest is opened.

**Debugging note** — burning one spider initially dissolved every spider in the scene,
because all instances shared one material. Fixed by instantiating `.material` (not
`.sharedMaterial`) per spider.

## Project structure

Assets/

    -   Script/ — gameplay scripts (SpiderBurn, ChestTrigger, PlayerPowers, etc.)
    -   Shader Folder/ — custom dissolve shader (Shader Graph)
    -   Scenes/ — main scene
    -   Settings/ — render pipeline / project settings
    -   FP Player/ — first-person controller (provided base scene)
    -   Models/ — scene models
    -   TextMesh Pro/ — Unity core package


## Third-party assets

This project uses the following Unity Asset Store assets, licensed under the Standard
Unity Asset Store EULA. Per the license terms, these are **not included** in this
repository — download them separately from the links below to run the project:

- [Fantasy Spider](https://assetstore.unity.com/packages/3d/characters/animals/insects/fantasy-spider-236418) — Dante's Anvil
- [Ocean & Lake Shaders | URP](https://assetstore.unity.com/packages/vfx/shaders/ocean-lake-shaders-urp-303983) — CodeDog0
- [AllSky Free - 10 Sky / Skybox Set](https://assetstore.unity.com/packages/2d/textures-materials/sky/allsky-free-10-sky-skybox-set-146014) — Rpgwhitelock
- [Stone Lantern Pack](https://assetstore.unity.com/packages/3d/environments/historic/stone-lantern-pack-139776) — Tanuki Digital
- [Door, Cabinets & Lockers (Free)](https://assetstore.unity.com/packages/audio/sound-fx/foley/door-cabinets-lockers-free-257610) — Syntoca

The base scene (island layout, crypt, fountain, first-person controller) was provided
as a starting point for the assignment.

## Files

- [`Shader_and_Environment_Design_Report.pdf`](./docs/Shader_and_Environment_Design_Report.pdf) — full report with methodology, shader graph breakdowns, and reflection

## Tools used

Unity, Shader Graph, C#