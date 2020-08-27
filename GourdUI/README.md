An experimental UI foundation for Unity.

GourdUI separates "screens" (logical UI pieces, such as a pause menu or a player HUD) from "views" (the visual assembalge). Screens can have multiple views implementing the same data/logic in different visual combinations - useful especially for multiplatform games, where the UI may need to look significantly different based on the runtime platform.

Using customizable filter data, GourdUI can automatically switch between views, selecting the one that best matches the current device/platform specifications. Views can be filtered based on platform, aspect ratio, and input types.

Because views are separate from screens, the logic that drives your UI can remain untouched while your designers go ham on different view configurations. Views only need to implement the data contract provided by the screen – how they do this is entirely up to that specific view.
