using Microsoft.Xna.Framework;
using MonoGame.Squid.Skinning;
using MonoGame.Squid.Structs;
using MonoGame.Squid.Util;
using s = MonoGame.Squid.Structs;

namespace temp1.UI
{
    static class Styling
    {
        public static Skin Skin { get; }
        public static CursorCollection Cursors { get; }
        static Styling()
        {
            var baseStyle = new ControlStyle();
            baseStyle.Tiling = TextureMode.Grid;
            baseStyle.Grid = new Margin(3);
            baseStyle.Texture = "button_default.dds";
            baseStyle.Hot.Texture = "button_hot.dds";
            baseStyle.Default.Texture = "button_default.dds";
            baseStyle.Pressed.Texture = "button_down.dds";
            baseStyle.Focused.Texture = "button_hot.dds";
            baseStyle.SelectedPressed.Texture = "button_down.dds";
            baseStyle.SelectedFocused.Texture = "button_hot.dds";
            baseStyle.Selected.Texture = "button_hot.dds";
            baseStyle.SelectedHot.Texture = "button_hot.dds";
            baseStyle.CheckedPressed.Texture = "button_down.dds";
            baseStyle.CheckedFocused.Texture = "button_down.dds";
            baseStyle.Checked.Texture = "button_down.dds";
            baseStyle.CheckedHot.Texture = "button_down.dds";

            var itemStyle = new ControlStyle(baseStyle);
            itemStyle.TextPadding = new Margin(10, 0, 0, 0);
            itemStyle.TextAlign = Alignment.MiddleLeft;

            var buttonStyle = new ControlStyle(baseStyle);
            buttonStyle.TextPadding = new Margin(10);
            buttonStyle.TextAlign = Alignment.MiddleCenter;

            var labelStyle = new ControlStyle();
            labelStyle.TextPadding = new Margin(8, 0, 8, 0);
            labelStyle.TextAlign = Alignment.MiddleLeft;
            labelStyle.BackColor = ColorInt.FromColor(Color.Gray);
            labelStyle.Default.BackColor = 0;

            var mainButtonStyle = new ControlStyle(buttonStyle);
            mainButtonStyle.Font = Font.Subtitle;

            var tooltipStyle = new ControlStyle(buttonStyle);
            tooltipStyle.TextPadding = new Margin(8);
            tooltipStyle.TextAlign = Alignment.TopLeft;
            tooltipStyle.Texture = "border.dds";
            tooltipStyle.Tiling = TextureMode.Grid;
            tooltipStyle.Grid = new Margin(3);
            tooltipStyle.BackColor = ColorInt.Argb(0, 0, 0, .9f);

            var inputStyle = new ControlStyle();
            inputStyle.Texture = "input_default.dds";
            inputStyle.Hot.Texture = "input_focused.dds";
            inputStyle.Focused.Texture = "input_focused.dds";
            inputStyle.TextPadding = new Margin(8);
            inputStyle.Tiling = TextureMode.Grid;
            inputStyle.Focused.Tint = ColorInt.Argb(1, 0, 0, 1);
            inputStyle.Grid = new Margin(3);

            var windowStyle = new ControlStyle();
            windowStyle.Tiling = TextureMode.Grid;
            windowStyle.Grid = new Margin(12);
            windowStyle.Texture = "window.dds";
            windowStyle.BackColor = ColorInt.Argb(0, 0, 0, .9f);

            var frameStyle = new ControlStyle();
            frameStyle.Tiling = TextureMode.Grid;
            frameStyle.Grid = new Margin(2);
            frameStyle.Texture = "frame.dds";
            frameStyle.TextPadding = new Margin(8);

            var vscrollTrackStyle = new ControlStyle();
            vscrollTrackStyle.Tiling = TextureMode.Grid;
            vscrollTrackStyle.Grid = new Margin(3);
            vscrollTrackStyle.Texture = "vscroll_track.dds";

            var vscrollButtonStyle = new ControlStyle();
            vscrollButtonStyle.Tiling = TextureMode.Grid;
            vscrollButtonStyle.Grid = new Margin(4);
            vscrollButtonStyle.Texture = "vscroll_button.dds";
            vscrollButtonStyle.Hot.Texture = "vscroll_button_hot.dds";
            vscrollButtonStyle.Pressed.Texture = "vscroll_button_down.dds";

            var vscrollUp = new ControlStyle();
            vscrollUp.Default.Texture = "vscrollUp_default.dds";
            vscrollUp.Hot.Texture = "vscrollUp_hot.dds";
            vscrollUp.Pressed.Texture = "vscrollUp_down.dds";
            vscrollUp.Focused.Texture = "vscrollUp_hot.dds";

            var hscrollTrackStyle = new ControlStyle();
            hscrollTrackStyle.Tiling = TextureMode.Grid;
            hscrollTrackStyle.Grid = new Margin(3);
            hscrollTrackStyle.Texture = "hscroll_track.dds";

            var hscrollButtonStyle = new ControlStyle();
            hscrollButtonStyle.Tiling = TextureMode.Grid;
            hscrollButtonStyle.Grid = new Margin(3);
            hscrollButtonStyle.Texture = "hscroll_button.dds";
            hscrollButtonStyle.Hot.Texture = "hscroll_button_hot.dds";
            hscrollButtonStyle.Pressed.Texture = "hscroll_button_down.dds";

            var hscrollUp = new ControlStyle();
            hscrollUp.Default.Texture = "hscrollUp_default.dds";
            hscrollUp.Hot.Texture = "hscrollUp_hot.dds";
            hscrollUp.Pressed.Texture = "hscrollUp_down.dds";
            hscrollUp.Focused.Texture = "hscrollUp_hot.dds";

            var checkButtonStyle = new ControlStyle();
            checkButtonStyle.Default.Texture = "checkbox_default.dds";
            checkButtonStyle.Hot.Texture = "checkbox_hot.dds";
            checkButtonStyle.Pressed.Texture = "checkbox_down.dds";
            checkButtonStyle.Checked.Texture = "checkbox_checked.dds";
            checkButtonStyle.CheckedFocused.Texture = "checkbox_checked_hot.dds";
            checkButtonStyle.CheckedHot.Texture = "checkbox_checked_hot.dds";
            checkButtonStyle.CheckedPressed.Texture = "checkbox_down.dds";

            var comboLabelStyle = new ControlStyle();
            comboLabelStyle.TextPadding = new Margin(10, 0, 0, 0);
            comboLabelStyle.Default.Texture = "combo_default.dds";
            comboLabelStyle.Hot.Texture = "combo_hot.dds";
            comboLabelStyle.Pressed.Texture = "combo_down.dds";
            comboLabelStyle.Focused.Texture = "combo_hot.dds";
            comboLabelStyle.Tiling = TextureMode.Grid;
            comboLabelStyle.Grid = new Margin(3, 0, 0, 0);

            var comboButtonStyle = new ControlStyle();
            comboButtonStyle.Default.Texture = "combo_button_default.dds";
            comboButtonStyle.Hot.Texture = "combo_button_hot.dds";
            comboButtonStyle.Pressed.Texture = "combo_button_down.dds";
            comboButtonStyle.Focused.Texture = "combo_button_hot.dds";

            var borderStyle = new ControlStyle();
            borderStyle.Texture = "border.dds";
            borderStyle.Tiling = TextureMode.Grid;
            borderStyle.Grid = new Margin(3);

            var titleStyle = new ControlStyle();
            titleStyle.TextAlign = Alignment.TopCenter;
            titleStyle.TextPadding = new Margin(8);
            titleStyle.Font = Font.Title;

            var subtitleStyle = new ControlStyle();
            subtitleStyle.TextAlign = Alignment.TopCenter;
            subtitleStyle.TextPadding = new Margin(8);
            subtitleStyle.Font = Font.Subtitle;

            var handleNw = new ControlStyle();
            handleNw.Texture = "handleNW.dds";

            var handleNe = new ControlStyle();
            handleNe.Texture = "handleNE.dds";

            var skin = new Skin();

            skin.Add("item", itemStyle);
            skin.Add("textbox", inputStyle);
            skin.Add("button", buttonStyle);
            skin.Add("mainButton", mainButtonStyle);
            skin.Add("window", windowStyle);
            skin.Add("frame", frameStyle);
            skin.Add("checkBox", checkButtonStyle);
            skin.Add("comboLabel", comboLabelStyle);
            skin.Add("comboButton", comboButtonStyle);
            skin.Add("vscrollTrack", vscrollTrackStyle);
            skin.Add("vscrollButton", vscrollButtonStyle);
            skin.Add("vscrollUp", vscrollUp);
            skin.Add("hscrollTrack", hscrollTrackStyle);
            skin.Add("hscrollButton", hscrollButtonStyle);
            skin.Add("hscrollUp", hscrollUp);
            skin.Add("label", labelStyle);
            skin.Add("title", titleStyle);
            skin.Add("tooltip", tooltipStyle);
            skin.Add("border", borderStyle);
            skin.Add("handleNE", handleNe);
            skin.Add("handleNW", handleNw);
            Skin = skin;

            var collection = new CursorCollection();
            collection.Add(CursorNames.Default, new Cursor { Texture = "cursors/cursor_default.png", HotSpot = s.Point.Zero });
            collection.Add(CursorNames.Link, new Cursor { Texture = "cursors/cursor_pointer.png", HotSpot = s.Point.Zero });
            Cursors = collection;
        }
    }
}