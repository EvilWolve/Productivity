using UnityEditor;

namespace framework.editor.codetemplates
{
    public class CodeTemplatesMenuItems
    {
        private const string MENU_ITEM_PATH = "Assets/Create/";
        private const int MENU_ITEM_PRIORITY = 70;

		[MenuItem (MENU_ITEM_PATH + "MonoBehaviour", false, MENU_ITEM_PRIORITY)]
		private static void CreateNewMonoBehaviourTemplate()
		{
		    CodeTemplates.CreateFromTemplate (
				"NewMonoBehaviour.cs",
				CodeTemplates.CODE_TEMPLATES_ROOT + "Templates/NewMonoBehaviourTemplate.txt");
		}

		[MenuItem (MENU_ITEM_PATH + "Service", false, MENU_ITEM_PRIORITY)]
		private static void CreateNewServiceTemplate()
		{
		    CodeTemplates.CreateFromTemplate (
				"NewService.cs",
				CodeTemplates.CODE_TEMPLATES_ROOT + "Templates/NewServiceTemplate.txt");
		}

		[MenuItem (MENU_ITEM_PATH + "UnitTest", false, MENU_ITEM_PRIORITY)]
		private static void CreateNewUnitTestTemplate()
		{
		    CodeTemplates.CreateFromTemplate (
				"NewUnitTest.cs",
				CodeTemplates.CODE_TEMPLATES_ROOT + "Templates/NewUnitTestTemplate.txt");
		}

    }
}