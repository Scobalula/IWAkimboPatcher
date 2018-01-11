# IWAkimboPatcher

A tool to patch Akimbo SEAnim's from non-Treyarch Call of Duty Titles by renaming joints to disallow 
animations affecting other joints and to allow for seperate weapon models.

# Download

The latest version can be found on the [Releases Page](https://github.com/Scobalula/IWAkimboPatcher/releases).

# To use:

1. Start by setting up your weapon's models in Maya, the Right arm model will be the model you export,
for the left model you'll want to rename the following joints:

| Joint            | New Name         |
|------------------|------------------|
| tag_weapon/j_gun | tag_weapon_le    |
| tag_flash        | tag_flash_le     |
| tag_brass        | tag_brass_le     |
| Everything else  | Add 1 to the end |

Make sure to save the right/left arm as seperate Maya scenes as we'll use them later for the
animations.

To make this part easier, a MEL script is included to handle renaming the left arm model.

2. Import the 2 models into your rig's scene and connect them to the arm, with the right going to
t7:tag_weapon_right and the left going to t7:tag_weapon_left.

3. Drop the left/right (do each arm seperately) arm's animations onto the window with the correct arm
selected in the drop down box and choose where to save them to.

4. Import the animations as normal into Maya. For combined animations (pullout, etc.) that contain both
arm's animations simply import/blend the other animation.

5. Export "tag_torso/tag_cambone" and hierarchy as normal. For ADS anims, import any right arm animation
and export "tag_view/tag_torso".

# Donate

If you use this tool in any of your projects, it would be appreciated if you credit me.

If you'd like to support me even more, consider donating, I develop a lot of apps including IW Akimbo Patcher and majority are available free of charge with source code included:

[![Donate](https://img.shields.io/badge/Donate-PayPal-yellowgreen.svg)](https://www.paypal.me/scobalula)

