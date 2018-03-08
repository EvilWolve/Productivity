using UnityEditor;
using System.IO;
using System.Text;

namespace framework.codetemplates
{
    public class CodeTemplatesPostProcess : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            RegenerateCodeTemplateMenu ();
        }

        static void RegenerateCodeTemplateMenu()
        {
            string codeTemplatesMenuPath = Path.GetFullPath (CodeTemplates.CODE_TEMPLATES_ROOT + "CodeTemplatesMenuItems.cs");
            string codeTemplatesMenuRawPath = Path.GetFullPath (CodeTemplates.CODE_TEMPLATES_ROOT + "CodeTemplatesMenuItems.txt");

            string codeTemplatesMenuItemRawPath = Path.GetFullPath (CodeTemplates.CODE_TEMPLATES_ROOT + "TemplateMenuItem.txt");

            string templateContents = string.Empty;
            string templateNode = string.Empty;

            if (File.Exists (codeTemplatesMenuRawPath))
            {
                using (var t = new StreamReader (codeTemplatesMenuRawPath))
                {
                    templateContents = t.ReadToEnd ();
                }
            }

            if (File.Exists (codeTemplatesMenuItemRawPath))
            {
                using (var t = new StreamReader (codeTemplatesMenuItemRawPath))
                {
                    templateNode = t.ReadToEnd ();
                }
            }

            AssetNode[] nodes = GetAtPath (CodeTemplates.CODE_TEMPLATES_ROOT + "Templates");
            
            string completeCode = "";
            
            for (int i = 0; i < nodes.Length; i++)
            {
                var t = templateNode.Replace ("##MENUITEM##", nodes[i].MenuItemName);
                t = t.Replace ("##TEMPLATE##", nodes[i].TemplateName);

                completeCode += t;
            }

            string final = templateContents.Replace ("##CODE##", completeCode);
            UTF8Encoding encoding = new UTF8Encoding (true, false);
            using (var fileStream = new FileStream (codeTemplatesMenuPath, FileMode.Create))
            {
                fileStream.Write (encoding.GetBytes (final), 0, final.Length);
            }

            AssetDatabase.Refresh ();
        }

        public static AssetNode[] GetAtPath(string path)
        {
            string[] fileEntries = System.Array.FindAll (Directory.GetFiles (path), f => f.Contains (".meta") == false);
            AssetNode[] nodes = new AssetNode[fileEntries.Length];

            for (int i = 0; i < fileEntries.Length; i++)
            {
                FileInfo info = new FileInfo (fileEntries[i]);
                string templateName = info.Name.Substring (0, info.Name.IndexOf ('.'));
                string menuItemName = templateName.Replace ("Template", string.Empty);
                menuItemName = menuItemName.Replace ("New", string.Empty);

                nodes[i] = new AssetNode (menuItemName, templateName);
            }

            return nodes;
        }
    }

    public struct AssetNode
    {
        public string MenuItemName;
        public string TemplateName;
        public AssetNode(string MenuItemName, string TemplateName)
        {
            this.MenuItemName = MenuItemName;
            this.TemplateName = TemplateName;
        }
    }
}