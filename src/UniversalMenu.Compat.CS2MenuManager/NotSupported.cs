using System;
using System.Collections.Generic;
using System.Drawing;

using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Timers;
using CounterStrikeSharp.API.Modules.Utils;

using CS2MenuManager.API.Class;
using CS2MenuManager.API.Enum;
using CS2MenuManager.API.Interface;

namespace CS2MenuManager
{
	public static class ProjectInfo
	{
		public const string Version = "v37";
		public const string Author = "Ashleigh Adams";
	}
}

namespace CS2MenuManager.API.Class
{
	public abstract class BaseMenu : IMenu
	{
		public string Title
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public List<ItemOption> ItemOptions
		{
			get => throw new NotImplementedException();
			internal set => throw new NotImplementedException();
		}
		public bool ExitButton
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public int MenuTime
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public IMenu? PrevMenu
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public BasePlugin Plugin
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public char ChatMenu_TitleColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public char ChatMenu_EnabledColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public char ChatMenu_DisabledColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public char ChatMenu_PrevPageColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public char ChatMenu_NextPageColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public char ChatMenu_ExitColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string CenterHtmlMenu_TitleColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string CenterHtmlMenu_EnabledColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string CenterHtmlMenu_DisabledColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string CenterHtmlMenu_PrevPageColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string CenterHtmlMenu_NextPageColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string CenterHtmlMenu_ExitColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public bool CenterHtmlMenu_InlinePageOptions
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public int CenterHtmlMenu_MaxTitleLength
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public int CenterHtmlMenu_MaxOptionLength
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_TitleColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_ScrollUpDownKeyColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_SelectKeyColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_PrevKeyColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_ExitKeyColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_SelectedOptionColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_OptionColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_DisabledOptionColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_ArrowColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public bool WasdMenu_FreezePlayer
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_ScrollUpKey
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_ScrollDownKey
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_SelectKey
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_PrevKey
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string WasdMenu_ExitKey
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public Color ScreenMenu_TextColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public Color ScreenMenu_DisabledTextColor
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string ScreenMenu_Font
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public int ScreenMenu_Size
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public bool ScreenMenu_FreezePlayer
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public bool ScreenMenu_ShowResolutionsOption
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string ScreenMenu_ScrollUpKey
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string ScreenMenu_ScrollDownKey
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string ScreenMenu_ExitKey
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public string ScreenMenu_SelectKey
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public MenuType ScreenMenu_MenuType
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		protected BaseMenu(string title, BasePlugin plugin)
		{
			throw new NotImplementedException();
		}
		public virtual ItemOption AddItem(string display, Action<CCSPlayerController, ItemOption> onSelect, DisableOption disableOption = DisableOption.None) => throw new NotImplementedException();
		public virtual ItemOption AddItem(string display, DisableOption disableOption) => throw new NotImplementedException();
		public abstract void Display(CCSPlayerController player, int time);
		public abstract void DisplayAt(CCSPlayerController player, int firstItem, int time);
		public void DisplayToAll(int time) => throw new NotImplementedException();
		public void DisplayAtToAll(int firstItem, int time) => throw new NotImplementedException();
	}

	public abstract class BaseMenuInstance : IMenuInstance, IDisposable
	{
		public CCSPlayerController Player { get; }
		public int Page => throw new NotImplementedException();
		public int CurrentOffset => throw new NotImplementedException();
		public virtual int NumPerPage => throw new NotImplementedException();
		protected virtual int MenuItemsPerPage => throw new NotImplementedException();
		public Stack<int> PrevPageOffsets => throw new NotImplementedException();
		public IMenu Menu { get; }
		protected bool HasPrevButton => throw new NotImplementedException();
		protected virtual bool HasNextButton => throw new NotImplementedException();
		protected bool HasExitButton => Menu.ExitButton;
		protected BaseMenuInstance(CCSPlayerController player, IMenu menu)
		{
			Player = player;
			Menu = menu;
		}
		public void NextPage() => throw new NotImplementedException();
		public void PrevPage() => throw new NotImplementedException();
		public virtual void Reset() => throw new NotImplementedException();
		public virtual void Close(bool exitSound) => throw new NotImplementedException();
		public virtual void Display() => throw new NotImplementedException();
		public virtual void OnKeyPress(CCSPlayerController player, int key) => throw new NotImplementedException();
		void IDisposable.Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing) => throw new NotImplementedException();
	}

	public abstract class BaseVote : IVoteMenu
	{
		public string Title { get; }
		public string Details { get; }
		public CCSPlayerController? VoteCaller { get; set; }
		public BaseVoteInstance.YesNoVoteResult Result { get; }
		public BaseVoteInstance.YesNoVoteHandler? Handler { get; }
		public BasePlugin Plugin { get; }
		public int VoteTime { get; protected set; } = 20;

		public BaseVote(string title, string details, BaseVoteInstance.YesNoVoteResult resultCallback, BaseVoteInstance.YesNoVoteHandler? handler, BasePlugin plugin)
		{
			Title = title;
			Details = details;
			Result = resultCallback;
			Handler = handler;
			Plugin = plugin;
		}

		public abstract void DisplayVoteToAll(int time);
	}

	public abstract class BaseVoteInstance : IVoteMenuInstance, IDisposable
	{
		public delegate void YesNoVoteHandler(YesNoVoteAction action, int param1, CastVote param2);
		public delegate bool YesNoVoteResult(YesNoVoteInfo info);
		IVoteMenu IVoteMenuInstance.VoteMenu => throw new NotImplementedException();
		public CVoteController VoteController => throw new NotImplementedException();
		public RecipientFilter CurrentVotefilter => throw new NotImplementedException();
		public Timer? Timer { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
		public int VoteCount { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
		public int VoterCount { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
		public int[] Voters { get => throw new NotImplementedException(); protected set => throw new NotImplementedException(); }
		protected BaseVoteInstance(List<CCSPlayerController> players, IVoteMenu menu) => throw new NotImplementedException();
		public virtual void Close() => throw new NotImplementedException();
		public virtual void Display() => throw new NotImplementedException();
		void IDisposable.Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing) => throw new NotImplementedException();
	}

	public static class Buttons
	{
		public static readonly IReadOnlyDictionary<string, PlayerButtons> ButtonMapping = new Dictionary<string, PlayerButtons>
		{
			["Alt1"] = (PlayerButtons)16384,
			["Alt2"] = (PlayerButtons)32768,
			["Attack"] = (PlayerButtons)1,
			["Attack2"] = (PlayerButtons)2048,
			["Attack3"] = (PlayerButtons)16777216,
			["Bullrush"] = (PlayerButtons)2097152,
			["Cancel"] = (PlayerButtons)64,
			["Duck"] = (PlayerButtons)4,
			["Grenade1"] = (PlayerButtons)4194304,
			["Grenade2"] = (PlayerButtons)8388608,
			["Space"] = (PlayerButtons)2,
			["Left"] = (PlayerButtons)128,
			["W"] = (PlayerButtons)8,
			["A"] = (PlayerButtons)512,
			["S"] = (PlayerButtons)16,
			["D"] = (PlayerButtons)1024,
			["E"] = (PlayerButtons)32,
			["R"] = (PlayerButtons)8192,
			["F"] = (PlayerButtons)34359738368L,
			["Shift"] = (PlayerButtons)65536,
			["Right"] = (PlayerButtons)256,
			["Run"] = (PlayerButtons)4096,
			["Walk"] = (PlayerButtons)131072,
			["Weapon1"] = (PlayerButtons)524288,
			["Weapon2"] = (PlayerButtons)1048576,
			["Zoom"] = (PlayerButtons)262144,
			["Tab"] = (PlayerButtons)8589934592L,
		};
	}

	public class ItemOption
	{
		public ItemOption(string display, DisableOption option, Action<CCSPlayerController, ItemOption>? onSelect)
		{
			Text = display;
			DisableOption = option;
			PostSelectAction = PostSelectAction.Nothing;
			OnSelect = onSelect;
		}
		public string Text
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public DisableOption DisableOption
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public PostSelectAction PostSelectAction
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
		public Action<CCSPlayerController, ItemOption>? OnSelect
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}
	}

	public static class MenuManager
	{
		public static readonly Dictionary<string, Type> MenuTypesList = new();
		public static IMenuInstance? GetActiveMenu(CCSPlayerController player) => throw new NotImplementedException();
		public static void CloseActiveMenu(CCSPlayerController player) => throw new NotImplementedException();
		public static void OpenMenu<TMenu>(CCSPlayerController player, TMenu menu, int? firstItem, Func<CCSPlayerController, TMenu, IMenuInstance> createInstance) where TMenu : IMenu => throw new NotImplementedException();
		public static void OnKeyPress(CCSPlayerController player, int key) => throw new NotImplementedException();
		public static T CreateMenu<T>(string title, BasePlugin plugin) where T : BaseMenu => (T)MenuByType(typeof(T), title, plugin);
		public static BaseMenu MenuByType(Type menuType, string title, BasePlugin plugin) => throw new NotImplementedException();
		public static BaseMenu MenuByType(string menuType, string title, BasePlugin plugin) => throw new NotImplementedException();
		public static void ReloadConfig() => throw new NotImplementedException();
	}

	public static class MenuTypeManager
	{
		public static Type GetDefaultMenu() => throw new NotImplementedException();
		public static Type? GetPlayerMenuType(CCSPlayerController player) => throw new NotImplementedException();
		public static Type SetPlayerMenuType(CCSPlayerController player, Type menuType) => throw new NotImplementedException();
		public static void MenuTypeMenu<T>(CCSPlayerController player, BasePlugin plugin, IMenu? prevMenu) where T : BaseMenu => throw new NotImplementedException();
		public static BaseMenu MenuTypeMenuByType(Type type, CCSPlayerController player, BasePlugin plugin, IMenu? prevMenu) => throw new NotImplementedException();
		public static BaseMenu MenuTypeMenuByType(string type, CCSPlayerController player, BasePlugin plugin, IMenu? prevMenu) => throw new NotImplementedException();
		public static void AddMenuTypeMenuItems(BaseMenu menu, IMenu? prevMenu) => throw new NotImplementedException();
	}


	public static class ResolutionManager
	{
		public class Resolution
		{
			public float PositionX;
			public float PositionY;
		}
		public static Resolution GetDefaultResolution() => throw new NotImplementedException();
		public static Resolution GetPlayerResolution(CCSPlayerController player) => throw new NotImplementedException();
		public static void SetPlayerResolution(CCSPlayerController player, Resolution resolution) => throw new NotImplementedException();
		public static T ResolutionMenu<T>(CCSPlayerController player, BasePlugin plugin, IMenu? prevMenu) where T : BaseMenu => throw new NotImplementedException();
		public static BaseMenu ResolutionMenuByType(Type type, CCSPlayerController player, BasePlugin plugin, IMenu? prevMenu) => throw new NotImplementedException();
		public static BaseMenu ResolutionMenuByType(string type, CCSPlayerController player, BasePlugin plugin, IMenu? prevMenu) => throw new NotImplementedException();
	}

	public static class VoteManager
	{
		public static bool IsVoteActive => throw new NotImplementedException();
		public static void CancelActiveVote() => throw new NotImplementedException();
		public static void OpenVoteMenu<TMenu>(List<CCSPlayerController> players, TMenu menu, Func<List<CCSPlayerController>, TMenu, IVoteMenuInstance> createInstance) where TMenu : IVoteMenu => throw new NotImplementedException();
	}

	public class YesNoVoteInfo
	{
		public int TotalVotes;
		public int YesVotes;
		public int NoVotes;
		public int TotalClients;
		public Dictionary<int, (int, int)> ClientInfo = new();
	}
}

namespace CS2MenuManager.API.Enum
{
	public enum CastVote
	{
		VOTE_NOTINCLUDED = -1,
		VOTE_OPTION1,
		VOTE_OPTION2,
		VOTE_OPTION3,
		VOTE_OPTION4,
		VOTE_OPTION5,
		VOTE_UNCAST
	}

	public enum DisableOption
	{
		None,
		DisableShowNumber,
		DisableHideNumber
	}

	public enum MenuType
	{
		Scrollable,
		KeyPress,
		Both
	}

	public enum PostSelectAction
	{
		Close,
		Reset,
		Nothing
	}

	public enum YesNoVoteAction
	{
		VoteAction_Start,
		VoteAction_Vote,
		VoteAction_End
	}
	public enum YesNoVoteEndReason
	{
		VoteEnd_AllVotes,
		VoteEnd_TimeUp,
		VoteEnd_Cancelled
	}
}

namespace CS2MenuManager.API.Interface
{
	public interface IMenu
	{
		string Title { get; set; }
		List<ItemOption> ItemOptions { get; }
		bool ExitButton { get; set; }
		int MenuTime { get; set; }
		IMenu? PrevMenu { get; set; }
		BasePlugin Plugin { get; }
		ItemOption AddItem(string display, Action<CCSPlayerController, ItemOption> onSelect, DisableOption disableOption = DisableOption.None);
		ItemOption AddItem(string display, DisableOption disableOption);
		void Display(CCSPlayerController player, int time);
		void DisplayAt(CCSPlayerController player, int firstItem, int time);
		void DisplayToAll(int time);
		void DisplayAtToAll(int firstItem, int time);
	}
	public interface IMenuInstance : IDisposable
	{
		CCSPlayerController Player { get; }
		int Page { get; }
		int CurrentOffset { get; }
		int NumPerPage { get; }
		Stack<int> PrevPageOffsets { get; }
		IMenu Menu { get; }
		void NextPage();
		void PrevPage();
		void Reset();
		void Close(bool exitSound);
		void Display();
		void OnKeyPress(CCSPlayerController player, int key);
	}

	public interface IVoteMenu
	{
		string Title { get; }
		string Details { get; }
		CCSPlayerController? VoteCaller { get; }
		BaseVoteInstance.YesNoVoteResult Result { get; }
		BaseVoteInstance.YesNoVoteHandler? Handler { get; }
		BasePlugin Plugin { get; }
		int VoteTime { get; }
		void DisplayVoteToAll(int time);
	}

	public interface IVoteMenuInstance : IDisposable
	{
		IVoteMenu VoteMenu { get; }
		CVoteController? VoteController { get; }
		RecipientFilter CurrentVotefilter { get; }
		Timer? Timer { get; }
		int VoteCount { get; }
		int VoterCount { get; }
		int[] Voters { get; }
		void Close();
		void Display();
	}
}

namespace CS2MenuManager.API.Menu
{
	public class CenterHtmlMenu : BaseMenu
	{
		public CenterHtmlMenu(string title, BasePlugin plugin) : base(title, plugin)
		{
			throw new NotImplementedException();
		}

		public override void Display(CCSPlayerController player, int time) => throw new NotImplementedException();

		public override void DisplayAt(CCSPlayerController player, int firstItem, int time) => throw new NotImplementedException();
	}

	public class CenterHtmlMenuInstance : BaseMenuInstance
	{
		public override int NumPerPage => throw new NotImplementedException();
		protected override int MenuItemsPerPage => throw new NotImplementedException();
		protected override bool HasNextButton => throw new NotImplementedException();
		public CenterHtmlMenuInstance(CCSPlayerController player, IMenu menu) :
			base(player, menu)
		{
			throw new NotImplementedException();
		}
		public override void Display() => throw new NotImplementedException();
		public override void Close(bool exitSound) => throw new NotImplementedException();
	}
	public class ChatMenu : BaseMenu
	{
		public ChatMenu(string title, BasePlugin plugin) :
			base(title, plugin)
		{
		}

		public override void Display(CCSPlayerController player, int time) => throw new NotImplementedException();
		public override void DisplayAt(CCSPlayerController player, int firstItem, int time) => throw new NotImplementedException();
	}
	public class ChatMenuInstance : BaseMenuInstance
	{
		public ChatMenuInstance(CCSPlayerController player, ChatMenu menu) :
				base(player, menu)
		{
		}
		public override void Display() => throw new NotImplementedException();
	}

	public class ConsoleMenu : BaseMenu
	{
		public ConsoleMenu(string title, BasePlugin plugin) :
			base(title, plugin)
		{
		}
		public override void Display(CCSPlayerController player, int time) => throw new NotImplementedException();
		public override void DisplayAt(CCSPlayerController player, int firstItem, int time) => throw new NotImplementedException();
	}

	public class ConsoleMenuInstance : BaseMenuInstance
	{
		public ConsoleMenuInstance(CCSPlayerController player, IMenu menu) :
			base(player, menu)
		{
		}
		public override void Display() => throw new NotImplementedException();
	}

	public class PanoramaVote : BaseVote
	{
		public PanoramaVote(
			string title,
			string details,
			BaseVoteInstance.YesNoVoteResult resultCallback,
			BaseVoteInstance.YesNoVoteHandler? handler,
			BasePlugin plugin
		) : base(
			title,
			details,
			resultCallback,
			handler,
			plugin)
		{
		}
		public override void DisplayVoteToAll(int time) => throw new NotImplementedException();
	}

	public class PanoramaVoteInstance : BaseVoteInstance
	{
		public PanoramaVoteInstance(List<CCSPlayerController> players, PanoramaVote menu) :
			base(players, menu)
		{
		}
		public override void Close() => throw new NotImplementedException();
		public override void Display() => throw new NotImplementedException();

	}

	public class PlayerMenu : BaseMenu
	{
		public PlayerMenu(string title, BasePlugin plugin) :
			base(title, plugin)
		{
		}
		public override void Display(CCSPlayerController player, int time) => throw new NotImplementedException();
		public override void DisplayAt(CCSPlayerController player, int firstItem, int time) => throw new NotImplementedException();
	}

	public class ScreenMenu : BaseMenu
	{
		public ScreenMenu(string title, BasePlugin plugin) :
			base(title, plugin)
		{
		}
		public override void Display(CCSPlayerController player, int time) => throw new NotImplementedException();
		public override void DisplayAt(CCSPlayerController player, int firstItem, int time) => throw new NotImplementedException();
	}

	public class ScreenMenuInstance : BaseMenuInstance
	{
		public override int NumPerPage => throw new NotImplementedException();
		public ScreenMenuInstance(CCSPlayerController player, IMenu menu) :
			base(player, menu)
		{
		}
		public override void Display() => throw new NotImplementedException();
		public override void Close(bool exitSound) => throw new NotImplementedException();
		public override void OnKeyPress(CCSPlayerController player, int key) => throw new NotImplementedException();
	}

	public class WasdMenu : BaseMenu
	{
		public WasdMenu(string title, BasePlugin plugin) :
			base(title, plugin)
		{
		}
		public override void Display(CCSPlayerController player, int time) => throw new NotImplementedException();
		public override void DisplayAt(CCSPlayerController player, int firstItem, int time) => throw new NotImplementedException();
	}

	public class WasdMenuInstance : BaseMenuInstance
	{
		public string DisplayString => throw new NotImplementedException();
		public override int NumPerPage => throw new NotImplementedException();
		public WasdMenuInstance(CCSPlayerController player, IMenu menu) :
			base(player, menu)
		{
		}
		public override void Display() => throw new NotImplementedException();
		public override void Close(bool exitSound) => throw new NotImplementedException();
		public void OnTick() => throw new NotImplementedException();
		public void Choose() => throw new NotImplementedException();
		public void ScrollDown() => throw new NotImplementedException();
		public void ScrollUp() => throw new NotImplementedException();
	}
}
