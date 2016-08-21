﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Runtime.Serialization;

namespace MailFetchingTool.Service_References.ServiceReference1 {
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name="MailboxInfoProxy", Namespace="http://schemas.datacontract.org/2004/07/BotWcfServiceIIS.MailBotServiceDTO")]
    [Serializable()]
    public partial class MailboxInfoProxy : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [NonSerialized()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [OptionalField()]
        private string PasswordField;
        
        [OptionalField()]
        private string UserNameField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [DataMember()]
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordField, value) != true)) {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }
        
        [DataMember()]
        public string UserName {
            get {
                return this.UserNameField;
            }
            set {
                if ((object.ReferenceEquals(this.UserNameField, value) != true)) {
                    this.UserNameField = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name="MailHostInfoProxy", Namespace="http://schemas.datacontract.org/2004/07/BotWcfServiceIIS.MailBotServiceDTO")]
    [Serializable()]
    public partial class MailHostInfoProxy : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [NonSerialized()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [OptionalField()]
        private string HostnameField;
        
        [OptionalField()]
        private int PortField;
        
        [OptionalField()]
        private MailProtocol ProtocolField;
        
        [OptionalField()]
        private bool UsingSslField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [DataMember()]
        public string Hostname {
            get {
                return this.HostnameField;
            }
            set {
                if ((object.ReferenceEquals(this.HostnameField, value) != true)) {
                    this.HostnameField = value;
                    this.RaisePropertyChanged("Hostname");
                }
            }
        }
        
        [DataMember()]
        public int Port {
            get {
                return this.PortField;
            }
            set {
                if ((this.PortField.Equals(value) != true)) {
                    this.PortField = value;
                    this.RaisePropertyChanged("Port");
                }
            }
        }
        
        [DataMember()]
        public MailProtocol Protocol {
            get {
                return this.ProtocolField;
            }
            set {
                if ((this.ProtocolField.Equals(value) != true)) {
                    this.ProtocolField = value;
                    this.RaisePropertyChanged("Protocol");
                }
            }
        }
        
        [DataMember()]
        public bool UsingSsl {
            get {
                return this.UsingSslField;
            }
            set {
                if ((this.UsingSslField.Equals(value) != true)) {
                    this.UsingSslField = value;
                    this.RaisePropertyChanged("UsingSsl");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name="MailProtocol", Namespace="http://schemas.datacontract.org/2004/07/BotWcfServiceIIS.MailBotServiceDTO")]
    public enum MailProtocol : int {
        
        [EnumMember()]
        Pop3 = 0,
        
        [EnumMember()]
        Imap = 1,
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [DataContract(Name="MailMessageProxy", Namespace="http://schemas.datacontract.org/2004/07/BotWcfServiceIIS.MailBotServiceDTO")]
    [Serializable()]
    public partial class MailMessageProxy : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [NonSerialized()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [OptionalField()]
        private string SubjectField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [DataMember()]
        public string Subject {
            get {
                return this.SubjectField;
            }
            set {
                if ((object.ReferenceEquals(this.SubjectField, value) != true)) {
                    this.SubjectField = value;
                    this.RaisePropertyChanged("Subject");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IBotMailService")]
    public interface IBotMailService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBotMailService/InitMessageReceive", ReplyAction="http://tempuri.org/IBotMailService/InitMessageReceiveResponse")]
        MailMessageProxy[] InitMessageReceive(MailboxInfoProxy mailboxInfo, MailHostInfoProxy hostInfo);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="http://tempuri.org/IBotMailService/InitMessageReceive", ReplyAction="http://tempuri.org/IBotMailService/InitMessageReceiveResponse")]
        System.IAsyncResult BeginInitMessageReceive(MailboxInfoProxy mailboxInfo, MailHostInfoProxy hostInfo, System.AsyncCallback callback, object asyncState);
        
        MailMessageProxy[] EndInitMessageReceive(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBotMailServiceChannel : IBotMailService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class InitMessageReceiveCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public InitMessageReceiveCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public MailMessageProxy[] Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((MailMessageProxy[])(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BotMailServiceClient : System.ServiceModel.ClientBase<IBotMailService>, IBotMailService {
        
        private BeginOperationDelegate onBeginInitMessageReceiveDelegate;
        
        private EndOperationDelegate onEndInitMessageReceiveDelegate;
        
        private System.Threading.SendOrPostCallback onInitMessageReceiveCompletedDelegate;
        
        public BotMailServiceClient() {
        }
        
        public BotMailServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public BotMailServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BotMailServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public BotMailServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public event System.EventHandler<InitMessageReceiveCompletedEventArgs> InitMessageReceiveCompleted;
        
        public MailMessageProxy[] InitMessageReceive(MailboxInfoProxy mailboxInfo, MailHostInfoProxy hostInfo) {
            return base.Channel.InitMessageReceive(mailboxInfo, hostInfo);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public System.IAsyncResult BeginInitMessageReceive(MailboxInfoProxy mailboxInfo, MailHostInfoProxy hostInfo, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginInitMessageReceive(mailboxInfo, hostInfo, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        public MailMessageProxy[] EndInitMessageReceive(System.IAsyncResult result) {
            return base.Channel.EndInitMessageReceive(result);
        }
        
        private System.IAsyncResult OnBeginInitMessageReceive(object[] inValues, System.AsyncCallback callback, object asyncState) {
            MailboxInfoProxy mailboxInfo = ((MailboxInfoProxy)(inValues[0]));
            MailHostInfoProxy hostInfo = ((MailHostInfoProxy)(inValues[1]));
            return this.BeginInitMessageReceive(mailboxInfo, hostInfo, callback, asyncState);
        }
        
        private object[] OnEndInitMessageReceive(System.IAsyncResult result) {
            MailMessageProxy[] retVal = this.EndInitMessageReceive(result);
            return new object[] {
                    retVal};
        }
        
        private void OnInitMessageReceiveCompleted(object state) {
            if ((this.InitMessageReceiveCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.InitMessageReceiveCompleted(this, new InitMessageReceiveCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void InitMessageReceiveAsync(MailboxInfoProxy mailboxInfo, MailHostInfoProxy hostInfo) {
            this.InitMessageReceiveAsync(mailboxInfo, hostInfo, null);
        }
        
        public void InitMessageReceiveAsync(MailboxInfoProxy mailboxInfo, MailHostInfoProxy hostInfo, object userState) {
            if ((this.onBeginInitMessageReceiveDelegate == null)) {
                this.onBeginInitMessageReceiveDelegate = new BeginOperationDelegate(this.OnBeginInitMessageReceive);
            }
            if ((this.onEndInitMessageReceiveDelegate == null)) {
                this.onEndInitMessageReceiveDelegate = new EndOperationDelegate(this.OnEndInitMessageReceive);
            }
            if ((this.onInitMessageReceiveCompletedDelegate == null)) {
                this.onInitMessageReceiveCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnInitMessageReceiveCompleted);
            }
            base.InvokeAsync(this.onBeginInitMessageReceiveDelegate, new object[] {
                        mailboxInfo,
                        hostInfo}, this.onEndInitMessageReceiveDelegate, this.onInitMessageReceiveCompletedDelegate, userState);
        }
    }
}
