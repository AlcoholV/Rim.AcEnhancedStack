using System.Linq;
using System.Reflection;
using Verse;

namespace AlcoholV
{
    [StaticConstructorOnStartup]
    internal static class AcEnhancedStack
    {
        static AcEnhancedStack()
        {
            LongEventHandler.QueueLongEvent(Inject, "Initializing", true, null);
        }

        private static Assembly Assembly
        {
            get { return Assembly.GetAssembly(typeof(AcEnhancedStack)); }
        }

        private static string AssemblyName
        {
            get { return Assembly.FullName.Split(',').First(); }
        }

        public static bool IsNeedIncreaedStack(ThingDef d)
        {
            return d.IsStuff || d.isBodyPartOrImplant || ((d.category == ThingCategory.Item) && !d.isUnfinishedThing && !d.IsCorpse && !d.destroyOnDrop && !d.IsRangedWeapon && !d.IsApparel && (d.stackLimit > 1));
        }

        private static void Inject()
        {
            var items = InjectDefs();
            Log.Message("Increased : " + items);
            Log.Message(AssemblyName + " injected.");
            
        }

        private static string InjectDefs()
        {
            var ret ="";

            var defs = DefDatabase<ThingDef>.AllDefs.Where(IsNeedIncreaedStack).ToList();
            foreach (var thingDef in defs)
            {
                thingDef.stackLimit = thingDef.stackLimit*10;
                thingDef.drawGUIOverlay = true; // force draw gui overlay for bodyparts
                ret += thingDef.defName+" ";
            }
            return ret;
        }
    }
}