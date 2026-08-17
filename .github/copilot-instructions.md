# GitHub Copilot Instructions for Desynchronized: Tales and News (Continued) Mod

## Mod Overview and Purpose

**Mod Name**: Desynchronized: Tales and News (Continued)  
**Author**: Vectorial1024

**Purpose**: This mod is a continuation of the "ClosedDoors" mod by MoobyNooby, aimed at enhancing the realism of how information spreads among colonists in RimWorld. The goal is to create a more immersive experience by implementing a realistic gossip system, where colonists only learn about events through physical proximity and communication, rather than omniscience.

## Key Features and Systems

- **Localized Knowledge**: Colonists only learn about events within their immediate vicinity, particularly in the same room or when in line of sight of the event, allowing a more realistic dispersion of information.
- **Inter-colonist Gossip**: Colonists have a chance to share important events during conversations, adding depth to interactions.
- **Trait Integration**: Supports varying reactions based on colonist traits, adding yet another layer of realism to their responses.
- **Event Prioritization**: Certain events are prioritized as "important" and are more likely to spread among colonists.

## Coding Patterns and Conventions

- **C# Structure**: The mod organizes its C# files primarily under the `Compatibility` folder to ensure that harmony patches and other integration scripts are easy to locate.
- **Class Naming**: Follow PascalCase for class names and methods. For instance, `Psycho_TraitDefOf` and `ModDetector`.
- **Method Use**: Utilize Harmony's `Postfix` and `Prepare` methods for modifying the game’s original functions.
- **File Organization**: Keep each type in its own file unless closely related to maintain readability and modularity of the code.

## XML Integration

- **Def Modifications**: Customize UI elements such as main buttons and pawn columns within XML files located in `/Defs/Misc/` and other directories like `/Defs/ThoughtDefs/` for thought modifications.
- **Stat and Thought Definitions**: XML definitions are heavily employed for adding new stats and thoughts, like the files found under `/Defs/Stats/` and `/Defs/ThoughtDefs/`.
- **Tag Usage**: Utilize XML tags consistently to ensure proper loading and reference of definitions.

## Harmony Patching

- **Purpose**: Used to inject additional behavior into existing game logic without modifying original game files.
- **Patch Locations**: Notable patches exist in files such as `TaleNewsPawnDied_TryProcessAsExecutionEvent.cs`, which handle specific event processing scenarios using the `Postfix` method for reactive changes.
- **Preparation Methods**: Implement `Prepare` methods to ensure patches are only applied when necessary, based on conditions like mod compatibility.

## Suggestions for Copilot

1. **Ensure XML Correctness**: When working with XML files, suggest using predefined XML schema from RimWorld for definitions like `MainButtonDef` or `ThoughtDef`.
2. **Code Completion**: Provide completion for often-used Harmony methods like `Postfix` and `Prepare` alongside explanations of when to use them.
3. **Error Prevention**: Recommend copilot to check the compatibility context, especially when these relate to known incompatible mods such as RuntimeGC.
4. **Enhancing Readability**: Guide users to separate their code by compatibility, utility, and core functionality, keeping in line with the organization found in `Desynchronized`.
5. **Documentation Assistance**: Suggest autocompleting method documentation by summarizing what each method does to ensure clarity for future collaborators.

By using these guidelines and structures, developers can continue to maintain and expand upon the "Desynchronized: Tales and News (Continued)" mod, ensuring it remains compatible, efficient, and a valuable addition to the RimWorld mod community.

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).

