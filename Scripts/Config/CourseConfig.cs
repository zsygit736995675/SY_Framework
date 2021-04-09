using UnityEngine;using Newtonsoft.Json;public class CourseConfig  {		/// <summary>		///课程		/// <summary>		public int id { get; set; }		/// <summary>		///名称		/// <summary>		public int nameid { get; set; }		/// <summary>		///题目数量		/// <summary>		public int quantity { get; set; }		/// <summary>		///是否顺序出题		/// <summary>		public bool isOrder { get; set; }		/// <summary>		///取值范围		/// <summary>		public int[] range { get; set; }		/// <summary>		///参与计算的数字		/// <summary>		public int[] count { get; set; }		/// <summary>		///展现方式(1,写数字版式，2图写数字版式3基础版式，4笔记本版式，5图加法版式6竖式版式，7长条版式，8非得数版式，9拆分版式。10乘法概念版式，11乘法表版式,)		/// <summary>		public int showType { get; set; }		/// <summary>		///出题方式（1计算得数，2计算被计算数字）		/// <summary>		public int[] questionType { get; set; }		 		public static string configName = "CourseConfig";		public static string Version { get { return Config.version; } }		public static CourseConfig [] Datas { get { return Config.datas; } }		public static CourseConfig Config {get { if (config == null) Init(); return config;}}		private static CourseConfig config;		public string version { get; set; }		public CourseConfig [] datas { get; set; }		private static void Init()		{			TextAsset jsonStr = Resources.Load("Config/"+configName) as TextAsset;			config = JsonConvert.DeserializeObject<CourseConfig>(jsonStr.text);		}		public static CourseConfig Get(int id)		{			foreach (var item in Config.datas)			{				if (item.id == id)				{ 					return item;				}			}			return null;		}		}