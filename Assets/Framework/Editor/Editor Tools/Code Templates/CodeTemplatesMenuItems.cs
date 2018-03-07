using UnityEditor;

namespace framework.codetemplates
{
    public class CodeTemplatesMenuItems
    {
        private const string MENU_ITEM_PATH = "Assets/Create/";
        private const int MENU_ITEM_PRIORITY = 70;

		[MenuItem (MENU_ITEM_PATH + "NewMonoBehaviour", false, MENU_ITEM_PRIORITY)]
		private static void CreateNewMonoBehaviour()
		{
		    CodeTemplates.CreateFromTemplate (
				"NewMonoBehaviour.cs",
				CodeTemplates.CODE_TEMPLATES_ROOT + "Templates/NewNewMonoBehaviour.txt");
		}

		[MenuItem (MENU_ITEM_PATH + "NewService", false, MENU_ITEM_PRIORITY)]
		private static void CreateNewService()
		{
		    CodeTemplates.CreateFromTemplate (
				"NewService.cs",
				CodeTemplates.CODE_TEMPLATES_ROOT + "Templates/NewNewService.txt");
		}

		[MenuItem (MENU_ITEM_PATH + "NewUnitTest", false, MENU_ITEM_PRIORITY)]
		private static void CreateNewUnitTest()
		{
		    CodeTemplates.CreateFromTemplate (
				"NewUnitTest.cs",
				CodeTemplates.CODE_TEMPLATES_ROOT + "Templates/NewNewUnitTest.txt");
		}

    }
}