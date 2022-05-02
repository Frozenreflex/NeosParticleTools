using HarmonyLib;
using NeosModLoader;
using FrooxEngine;
using BaseX;
using FrooxEngine.UIX;

namespace ParticleTools
{
    public class ParticleTools : NeosMod
    {
        public override string Name => "ParticleTools";
        public override string Author => "Fro Zen";
        public override string Version => "1.0.0";

        private static bool _first_trigger = false;

        public override void OnEngineInit()
        {
            var harmony = new Harmony("ParticleToolsHarmony");
            harmony.PatchAll();
        }
    }
    [HarmonyPatch(typeof(ParticleStyle))]
    public class ParticleStylePatch
    {
        [HarmonyPostfix]
        [HarmonyPatch("BuildInspectorUI")]
        public static void Postfix(UIBuilder ui, ref ParticleStyle __instance)
        {
            var style = __instance;
            ui.NestOut();
            ui.HorizontalLayout(4f);
            ui.Text("Fade In Value");
            ui.Text("Fade Out Value");
            
            ui.NestOut();
            ui.HorizontalLayout(4f);
            var customin = ui.FloatField(0, 1);
            var customout = ui.FloatField(0, 1);

            ui.NestOut();
            ui.HorizontalLayout(4f);
            var customfadeinout = ui.Button("Quick Alpha Fades");
            customfadeinout.LocalPressed += (button, data) => 
                style.SetupAlphaFadeInFadeOut(customin.ParsedValue, customout.ParsedValue);
            var customintensityinout = ui.Button("Quick Intensity Fades");
            customintensityinout.LocalPressed += (button, data) => 
                style.SetupIntensityFadeInFadeOut(customin.ParsedValue, customout.ParsedValue);
            
            ui.NestOut();
            ui.HorizontalLayout(4f);
            ui.Text("Fade Key Editor");
            
            //TODO: figure out how to allign the text properly
            
            ui.NestOut();
            ui.HorizontalLayout(4f);
            var colortext = ui.Text("Color", alignment: Alignment.MiddleLeft);
            var colorinR = ui.FloatField();
            var colorinG = ui.FloatField();
            var colorinB = ui.FloatField();
            colorinR.ParsedValue.Value = 1;
            colorinG.ParsedValue.Value = 1;
            colorinB.ParsedValue.Value = 1;

            ui.NestOut();
            ui.HorizontalLayout(4f);
            var alphatext = ui.Text("Alpha", alignment: Alignment.MiddleLeft);
            var alphain = ui.FloatField();
            
            ui.NestOut();
            ui.HorizontalLayout(4f);
            var positiontext = ui.Text("Position", alignment: Alignment.MiddleLeft);
            var position = ui.FloatField();
            
            ui.NestOut();
            ui.HorizontalLayout(4f);
            var alphakey = ui.Button("Add Alpha Key");
            alphakey.LocalPressed += (button, data) => 
                style.AlphaOverLifetime.InsertKey(position.ParsedValue, alphain.ParsedValue);
            var intensitykey = ui.Button("Add Intensity Key");
            intensitykey.LocalPressed += (button, data) => 
                style.ColorOverLifetime.InsertKey(position.ParsedValue, 
                    new color(colorinR.ParsedValue, colorinR.ParsedValue, colorinB.ParsedValue, 1));
        }
    }
}
