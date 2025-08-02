# GitHub Copilot Instructions

## Mod Overview and Purpose
**Mod Name:** Desynchronized: Tales and News (Continued)

This mod is a continuation of Vectorial1024's original project, aiming to provide a more immersive storytelling experience in RimWorld by altering how information spreads among colonists. It challenges the strange notion that colonists know about all events happening instantly across the map, promoting a more realistic simulation where information spreads through interaction and proximity.

## Key Features and Systems
- **Event Awareness:** Colonists will generally not react to off-map events, promoting a realistic awareness level.
- **News Spread:** News about past events can be spread orally through colonists during social interactions.
- **Event Significance:** Some news are rated as more important based on the colonist's personal values or traits.
- **Custom Events:** Support for new events such as Butchered Humanoid-news and Trait Value news have been added.

**Compatibility:**
- Requires HugsLib and should be loaded after it.
- May have compatibility issues with RuntimeGC-mods depending on their configuration.

## Coding Patterns and Conventions
- **Class and File Naming:** The project uses a clear and descriptive naming convention, for example, `PostFix_Desync_TNPawnDied_GenericThoughts`.
- **Method Naming:** Methods are typically action-oriented, e.g., `EstablishRelationship`, `ForgetRandomly`.
- **Access Modifiers:** All classes and methods are prefixed with appropriate access modifiers (e.g., `public`, `private`).

## XML Integration
The mod heavily uses XML files for defining game objects like traits and thoughts, following the RimWorld modding conventions. Ensure coherence between XML definitions and C# code implementation by maintaining naming consistency.

## Harmony Patching
The mod makes extensive use of Harmony patches to alter and extend the behavior of existing game systems. Ensure:
- **Non-Destructive Coding:** Harmony patches should be designed to enhance or extend functionalities without breaking the core functionality.
- **Prefix and Postfix:** Use prefix and postfix patches where appropriate. For example, `PostFix_Pawn_PreTraded` adjusts behavior after the trading event.
- **Transpilers:** Use for more complex changes to the IL code execution like bug fixes.

## Suggestions for Copilot
1. **Enhancing News Systems:** Implement new features that allow colonists to react gradually to news based on social skills or proximity.
2. **Event Details:** Add additional parameters to tailor event significance better, like emotional impact based on relationships.
3. **Performance Optimization:** Suggest ways to improve the performance of news spreading algorithms, particularly in very large colonies.
4. **Safety Checks:** Implement methods to verify data integrity, especially after game updates or when other mods interact with Desynchronized's features.

### Summary of Classes and Files
A detailed breakdown of C# classes and files is provided above, with key handlers for various pawn-related events and utilities for seamlessly integrating news systems. Developers are encouraged to explore these files for a deeper understanding of the mod's mechanics and extend them as necessary for new functionality.

---

Happy modding in the RimWorld universe! Your contributions could very well be the news among colonists.
