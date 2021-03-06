﻿#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3615
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CybersourceReportDownloader
{
	using System.Data.Linq;
	using System.Data.Linq.Mapping;
	using System.Data;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Linq;
	using System.Linq.Expressions;
	using System.ComponentModel;
	using System;
	
	
	[System.Data.Linq.Mapping.DatabaseAttribute(Name="Treasury")]
	public partial class TreasuryDataContext : System.Data.Linq.DataContext
	{
		
		private static System.Data.Linq.Mapping.MappingSource mappingSource = new AttributeMappingSource();
		
    #region Extensibility Method Definitions
    partial void OnCreated();
    partial void InsertActivityLog(ActivityLog instance);
    partial void UpdateActivityLog(ActivityLog instance);
    partial void DeleteActivityLog(ActivityLog instance);
    partial void InsertCybersourceGatewayMerchantConfig(CybersourceGatewayMerchantConfig instance);
    partial void UpdateCybersourceGatewayMerchantConfig(CybersourceGatewayMerchantConfig instance);
    partial void DeleteCybersourceGatewayMerchantConfig(CybersourceGatewayMerchantConfig instance);
    partial void InsertCybersourceGatewayTranDownloadTracking(CybersourceGatewayTranDownloadTracking instance);
    partial void UpdateCybersourceGatewayTranDownloadTracking(CybersourceGatewayTranDownloadTracking instance);
    partial void DeleteCybersourceGatewayTranDownloadTracking(CybersourceGatewayTranDownloadTracking instance);
    partial void InsertSTG_CybersourceTransaction(STG_CybersourceTransaction instance);
    partial void UpdateSTG_CybersourceTransaction(STG_CybersourceTransaction instance);
    partial void DeleteSTG_CybersourceTransaction(STG_CybersourceTransaction instance);
    #endregion
		
		public TreasuryDataContext() : 
				base(global::CybersourceReportDownloader.Properties.Settings.Default.TreasuryConnectionString, mappingSource)
		{
			OnCreated();
		}
		
		public TreasuryDataContext(string connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TreasuryDataContext(System.Data.IDbConnection connection) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TreasuryDataContext(string connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public TreasuryDataContext(System.Data.IDbConnection connection, System.Data.Linq.Mapping.MappingSource mappingSource) : 
				base(connection, mappingSource)
		{
			OnCreated();
		}
		
		public System.Data.Linq.Table<ActivityLog> ActivityLogs
		{
			get
			{
				return this.GetTable<ActivityLog>();
			}
		}
		
		public System.Data.Linq.Table<CybersourceGatewayMerchantConfig> CybersourceGatewayMerchantConfigs
		{
			get
			{
				return this.GetTable<CybersourceGatewayMerchantConfig>();
			}
		}
		
		public System.Data.Linq.Table<CybersourceGatewayTranDownloadTracking> CybersourceGatewayTranDownloadTrackings
		{
			get
			{
				return this.GetTable<CybersourceGatewayTranDownloadTracking>();
			}
		}
		
		public System.Data.Linq.Table<STG_CybersourceTransaction> STG_CybersourceTransactions
		{
			get
			{
				return this.GetTable<STG_CybersourceTransaction>();
			}
		}
	}
	
	[Table(Name="dbo.T_ActivityLog")]
	public partial class ActivityLog : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _id;
		
		private string _logger;
		
		private string _method_name;
		
		private System.Nullable<System.DateTime> _application_time;
		
		private string _message;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnidChanging(int value);
    partial void OnidChanged();
    partial void OnloggerChanging(string value);
    partial void OnloggerChanged();
    partial void Onmethod_nameChanging(string value);
    partial void Onmethod_nameChanged();
    partial void Onapplication_timeChanging(System.Nullable<System.DateTime> value);
    partial void Onapplication_timeChanged();
    partial void OnmessageChanging(string value);
    partial void OnmessageChanged();
    #endregion
		
		public ActivityLog()
		{
			OnCreated();
		}
		
		[Column(Storage="_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int id
		{
			get
			{
				return this._id;
			}
			set
			{
				if ((this._id != value))
				{
					this.OnidChanging(value);
					this.SendPropertyChanging();
					this._id = value;
					this.SendPropertyChanged("id");
					this.OnidChanged();
				}
			}
		}
		
		[Column(Storage="_logger", DbType="VarChar(50)")]
		public string logger
		{
			get
			{
				return this._logger;
			}
			set
			{
				if ((this._logger != value))
				{
					this.OnloggerChanging(value);
					this.SendPropertyChanging();
					this._logger = value;
					this.SendPropertyChanged("logger");
					this.OnloggerChanged();
				}
			}
		}
		
		[Column(Storage="_method_name", DbType="VarChar(100)")]
		public string method_name
		{
			get
			{
				return this._method_name;
			}
			set
			{
				if ((this._method_name != value))
				{
					this.Onmethod_nameChanging(value);
					this.SendPropertyChanging();
					this._method_name = value;
					this.SendPropertyChanged("method_name");
					this.Onmethod_nameChanged();
				}
			}
		}
		
		[Column(Storage="_application_time", DbType="DateTime")]
		public System.Nullable<System.DateTime> application_time
		{
			get
			{
				return this._application_time;
			}
			set
			{
				if ((this._application_time != value))
				{
					this.Onapplication_timeChanging(value);
					this.SendPropertyChanging();
					this._application_time = value;
					this.SendPropertyChanged("application_time");
					this.Onapplication_timeChanged();
				}
			}
		}
		
		[Column(Storage="_message", DbType="VarChar(1000)")]
		public string message
		{
			get
			{
				return this._message;
			}
			set
			{
				if ((this._message != value))
				{
					this.OnmessageChanging(value);
					this.SendPropertyChanging();
					this._message = value;
					this.SendPropertyChanged("message");
					this.OnmessageChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.T_CybersourceGatewayMerchantConfig")]
	public partial class CybersourceGatewayMerchantConfig : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _CybersourceGatewayMerchantConfigID;
		
		private System.Nullable<int> _CybersourceGatewayProcessorID;
		
		private string _MerchantId;
		
		private string _Username;
		
		private string _Password;
		
		private string _ProfileName;
		
		private bool _IgnoreCVResult;
		
		private bool _Level2or3Enabled;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCybersourceGatewayMerchantConfigIDChanging(int value);
    partial void OnCybersourceGatewayMerchantConfigIDChanged();
    partial void OnCybersourceGatewayProcessorIDChanging(System.Nullable<int> value);
    partial void OnCybersourceGatewayProcessorIDChanged();
    partial void OnMerchantIdChanging(string value);
    partial void OnMerchantIdChanged();
    partial void OnUsernameChanging(string value);
    partial void OnUsernameChanged();
    partial void OnPasswordChanging(string value);
    partial void OnPasswordChanged();
    partial void OnProfileNameChanging(string value);
    partial void OnProfileNameChanged();
    partial void OnIgnoreCVResultChanging(bool value);
    partial void OnIgnoreCVResultChanged();
    partial void OnLevel2or3EnabledChanging(bool value);
    partial void OnLevel2or3EnabledChanged();
    #endregion
		
		public CybersourceGatewayMerchantConfig()
		{
			OnCreated();
		}
		
		[Column(Storage="_CybersourceGatewayMerchantConfigID", DbType="Int NOT NULL", IsPrimaryKey=true)]
		public int CybersourceGatewayMerchantConfigID
		{
			get
			{
				return this._CybersourceGatewayMerchantConfigID;
			}
			set
			{
				if ((this._CybersourceGatewayMerchantConfigID != value))
				{
					this.OnCybersourceGatewayMerchantConfigIDChanging(value);
					this.SendPropertyChanging();
					this._CybersourceGatewayMerchantConfigID = value;
					this.SendPropertyChanged("CybersourceGatewayMerchantConfigID");
					this.OnCybersourceGatewayMerchantConfigIDChanged();
				}
			}
		}
		
		[Column(Storage="_CybersourceGatewayProcessorID", DbType="Int")]
		public System.Nullable<int> CybersourceGatewayProcessorID
		{
			get
			{
				return this._CybersourceGatewayProcessorID;
			}
			set
			{
				if ((this._CybersourceGatewayProcessorID != value))
				{
					this.OnCybersourceGatewayProcessorIDChanging(value);
					this.SendPropertyChanging();
					this._CybersourceGatewayProcessorID = value;
					this.SendPropertyChanged("CybersourceGatewayProcessorID");
					this.OnCybersourceGatewayProcessorIDChanged();
				}
			}
		}
		
		[Column(Storage="_MerchantId", DbType="VarChar(50) NOT NULL", CanBeNull=false)]
		public string MerchantId
		{
			get
			{
				return this._MerchantId;
			}
			set
			{
				if ((this._MerchantId != value))
				{
					this.OnMerchantIdChanging(value);
					this.SendPropertyChanging();
					this._MerchantId = value;
					this.SendPropertyChanged("MerchantId");
					this.OnMerchantIdChanged();
				}
			}
		}
		
		[Column(Storage="_Username", DbType="VarChar(100) NOT NULL", CanBeNull=false)]
		public string Username
		{
			get
			{
				return this._Username;
			}
			set
			{
				if ((this._Username != value))
				{
					this.OnUsernameChanging(value);
					this.SendPropertyChanging();
					this._Username = value;
					this.SendPropertyChanged("Username");
					this.OnUsernameChanged();
				}
			}
		}
		
		[Column(Storage="_Password", DbType="VarChar(500) NOT NULL", CanBeNull=false)]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				if ((this._Password != value))
				{
					this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}
			}
		}
		
		[Column(Storage="_ProfileName", DbType="VarChar(50)")]
		public string ProfileName
		{
			get
			{
				return this._ProfileName;
			}
			set
			{
				if ((this._ProfileName != value))
				{
					this.OnProfileNameChanging(value);
					this.SendPropertyChanging();
					this._ProfileName = value;
					this.SendPropertyChanged("ProfileName");
					this.OnProfileNameChanged();
				}
			}
		}
		
		[Column(Storage="_IgnoreCVResult", DbType="Bit NOT NULL")]
		public bool IgnoreCVResult
		{
			get
			{
				return this._IgnoreCVResult;
			}
			set
			{
				if ((this._IgnoreCVResult != value))
				{
					this.OnIgnoreCVResultChanging(value);
					this.SendPropertyChanging();
					this._IgnoreCVResult = value;
					this.SendPropertyChanged("IgnoreCVResult");
					this.OnIgnoreCVResultChanged();
				}
			}
		}
		
		[Column(Storage="_Level2or3Enabled", DbType="Bit NOT NULL")]
		public bool Level2or3Enabled
		{
			get
			{
				return this._Level2or3Enabled;
			}
			set
			{
				if ((this._Level2or3Enabled != value))
				{
					this.OnLevel2or3EnabledChanging(value);
					this.SendPropertyChanging();
					this._Level2or3Enabled = value;
					this.SendPropertyChanged("Level2or3Enabled");
					this.OnLevel2or3EnabledChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.T_CybersourceGatewayTranDownloadTracking")]
	public partial class CybersourceGatewayTranDownloadTracking : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _CybersourceGatewayTranDownloadTracking_id;
		
		private string _MerchantId;
		
		private string _UserName;
		
		private string _Password;
		
		private System.Nullable<System.DateTime> _ReportDate;
		
		private string _FileName;
		
		private System.Nullable<bool> _isDownLoadSuccess;
		
		private string _Message;
		
		private string _NewPasswordSet;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnCybersourceGatewayTranDownloadTracking_idChanging(int value);
    partial void OnCybersourceGatewayTranDownloadTracking_idChanged();
    partial void OnMerchantIdChanging(string value);
    partial void OnMerchantIdChanged();
    partial void OnUserNameChanging(string value);
    partial void OnUserNameChanged();
    partial void OnPasswordChanging(string value);
    partial void OnPasswordChanged();
    partial void OnReportDateChanging(System.Nullable<System.DateTime> value);
    partial void OnReportDateChanged();
    partial void OnFileNameChanging(string value);
    partial void OnFileNameChanged();
    partial void OnisDownLoadSuccessChanging(System.Nullable<bool> value);
    partial void OnisDownLoadSuccessChanged();
    partial void OnMessageChanging(string value);
    partial void OnMessageChanged();
    partial void OnNewPasswordSetChanging(string value);
    partial void OnNewPasswordSetChanged();
    #endregion
		
		public CybersourceGatewayTranDownloadTracking()
		{
			OnCreated();
		}
		
		[Column(Storage="_CybersourceGatewayTranDownloadTracking_id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int CybersourceGatewayTranDownloadTracking_id
		{
			get
			{
				return this._CybersourceGatewayTranDownloadTracking_id;
			}
			set
			{
				if ((this._CybersourceGatewayTranDownloadTracking_id != value))
				{
					this.OnCybersourceGatewayTranDownloadTracking_idChanging(value);
					this.SendPropertyChanging();
					this._CybersourceGatewayTranDownloadTracking_id = value;
					this.SendPropertyChanged("CybersourceGatewayTranDownloadTracking_id");
					this.OnCybersourceGatewayTranDownloadTracking_idChanged();
				}
			}
		}
		
		[Column(Storage="_MerchantId", DbType="VarChar(50)")]
		public string MerchantId
		{
			get
			{
				return this._MerchantId;
			}
			set
			{
				if ((this._MerchantId != value))
				{
					this.OnMerchantIdChanging(value);
					this.SendPropertyChanging();
					this._MerchantId = value;
					this.SendPropertyChanged("MerchantId");
					this.OnMerchantIdChanged();
				}
			}
		}
		
		[Column(Storage="_UserName", DbType="VarChar(100)")]
		public string UserName
		{
			get
			{
				return this._UserName;
			}
			set
			{
				if ((this._UserName != value))
				{
					this.OnUserNameChanging(value);
					this.SendPropertyChanging();
					this._UserName = value;
					this.SendPropertyChanged("UserName");
					this.OnUserNameChanged();
				}
			}
		}
		
		[Column(Storage="_Password", DbType="VarChar(100)")]
		public string Password
		{
			get
			{
				return this._Password;
			}
			set
			{
				if ((this._Password != value))
				{
					this.OnPasswordChanging(value);
					this.SendPropertyChanging();
					this._Password = value;
					this.SendPropertyChanged("Password");
					this.OnPasswordChanged();
				}
			}
		}
		
		[Column(Storage="_ReportDate", DbType="DateTime")]
		public System.Nullable<System.DateTime> ReportDate
		{
			get
			{
				return this._ReportDate;
			}
			set
			{
				if ((this._ReportDate != value))
				{
					this.OnReportDateChanging(value);
					this.SendPropertyChanging();
					this._ReportDate = value;
					this.SendPropertyChanged("ReportDate");
					this.OnReportDateChanged();
				}
			}
		}
		
		[Column(Storage="_FileName", DbType="VarChar(500)")]
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if ((this._FileName != value))
				{
					this.OnFileNameChanging(value);
					this.SendPropertyChanging();
					this._FileName = value;
					this.SendPropertyChanged("FileName");
					this.OnFileNameChanged();
				}
			}
		}
		
		[Column(Storage="_isDownLoadSuccess", DbType="Bit")]
		public System.Nullable<bool> isDownLoadSuccess
		{
			get
			{
				return this._isDownLoadSuccess;
			}
			set
			{
				if ((this._isDownLoadSuccess != value))
				{
					this.OnisDownLoadSuccessChanging(value);
					this.SendPropertyChanging();
					this._isDownLoadSuccess = value;
					this.SendPropertyChanged("isDownLoadSuccess");
					this.OnisDownLoadSuccessChanged();
				}
			}
		}
		
		[Column(Storage="_Message", DbType="VarChar(4000)")]
		public string Message
		{
			get
			{
				return this._Message;
			}
			set
			{
				if ((this._Message != value))
				{
					this.OnMessageChanging(value);
					this.SendPropertyChanging();
					this._Message = value;
					this.SendPropertyChanged("Message");
					this.OnMessageChanged();
				}
			}
		}
		
		[Column(Storage="_NewPasswordSet", DbType="VarChar(100)")]
		public string NewPasswordSet
		{
			get
			{
				return this._NewPasswordSet;
			}
			set
			{
				if ((this._NewPasswordSet != value))
				{
					this.OnNewPasswordSetChanging(value);
					this.SendPropertyChanging();
					this._NewPasswordSet = value;
					this.SendPropertyChanged("NewPasswordSet");
					this.OnNewPasswordSetChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
	
	[Table(Name="dbo.T_STG_CybersourceTransactions")]
	public partial class STG_CybersourceTransaction : INotifyPropertyChanging, INotifyPropertyChanged
	{
		
		private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
		
		private int _STG_CybersourceTransactions_Id;
		
		private string _Row_descriptor;
		
		private string _FileName;
		
    #region Extensibility Method Definitions
    partial void OnLoaded();
    partial void OnValidate(System.Data.Linq.ChangeAction action);
    partial void OnCreated();
    partial void OnSTG_CybersourceTransactions_IdChanging(int value);
    partial void OnSTG_CybersourceTransactions_IdChanged();
    partial void OnRow_descriptorChanging(string value);
    partial void OnRow_descriptorChanged();
    partial void OnFileNameChanging(string value);
    partial void OnFileNameChanged();
    #endregion
		
		public STG_CybersourceTransaction()
		{
			OnCreated();
		}
		
		[Column(Storage="_STG_CybersourceTransactions_Id", AutoSync=AutoSync.OnInsert, DbType="Int NOT NULL IDENTITY", IsPrimaryKey=true, IsDbGenerated=true)]
		public int STG_CybersourceTransactions_Id
		{
			get
			{
				return this._STG_CybersourceTransactions_Id;
			}
			set
			{
				if ((this._STG_CybersourceTransactions_Id != value))
				{
					this.OnSTG_CybersourceTransactions_IdChanging(value);
					this.SendPropertyChanging();
					this._STG_CybersourceTransactions_Id = value;
					this.SendPropertyChanged("STG_CybersourceTransactions_Id");
					this.OnSTG_CybersourceTransactions_IdChanged();
				}
			}
		}
		
		[Column(Storage="_Row_descriptor", DbType="VarChar(8000)")]
		public string Row_descriptor
		{
			get
			{
				return this._Row_descriptor;
			}
			set
			{
				if ((this._Row_descriptor != value))
				{
					this.OnRow_descriptorChanging(value);
					this.SendPropertyChanging();
					this._Row_descriptor = value;
					this.SendPropertyChanged("Row_descriptor");
					this.OnRow_descriptorChanged();
				}
			}
		}
		
		[Column(Storage="_FileName", DbType="NVarChar(2000)")]
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if ((this._FileName != value))
				{
					this.OnFileNameChanging(value);
					this.SendPropertyChanging();
					this._FileName = value;
					this.SendPropertyChanged("FileName");
					this.OnFileNameChanged();
				}
			}
		}
		
		public event PropertyChangingEventHandler PropertyChanging;
		
		public event PropertyChangedEventHandler PropertyChanged;
		
		protected virtual void SendPropertyChanging()
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, emptyChangingEventArgs);
			}
		}
		
		protected virtual void SendPropertyChanged(String propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
#pragma warning restore 1591
