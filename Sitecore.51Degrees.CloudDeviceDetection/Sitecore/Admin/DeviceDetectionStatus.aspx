<%@ Page Language="C#" AutoEventWireup="true" Debug="true" Inherits="Sitecore.sitecore.admin.AdminPage" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Sitecore.FiftyOneDegrees.CloudDeviceDetection.Factories" %>
<%@ Import Namespace="scapi=Sitecore"  %>

<script runat="server" type="text/C#">
    protected DeviceDetectionManagerStatus DeviceDetectionManager { get; set; }

    protected class DeviceDetectionManagerStatus
    {
        public bool Exists { get; set; }
        public bool Enabled { get; set; }
        public scapi.Caching.Cache Cache { get; set; }
    }

    protected override void OnInit(System.EventArgs arguments)
    {
        CheckSecurity(true);
    }

    protected void Page_Load(object sender, EventArgs args)
    {
        if (!IsPostBack)
        {
            var userAgent = HttpContext.Current.Request.UserAgent;

            if(HttpContext.Current.Request.QueryString.AllKeys.Contains("useragent"))
            {
                userAgent = HttpUtility.UrlDecode(HttpContext.Current.Request.QueryString["useragent"]);
            }

            SetBrowserCapabilitiesRepeater(userAgent);

            DeviceDetectionManager = new DeviceDetectionManagerStatus();

            var deviceDetectionAssemblyExists = SitecoreCesDeviceDetectionExists();
            DeviceDetectionManagerDetailsPanel.Visible = deviceDetectionAssemblyExists;
#if deviceDetectionAssemblyExists

            var deviceDetectionAssembly = System.Reflection.Assembly.Load("Sitecore.CES.DeviceDetection");
            DeviceDetectionManager.Exists = deviceDetectionAssembly != null && deviceDetectionAssembly.GetType("Sitecore.CES.DeviceDetection.DeviceDetectionManager") != null;

            if(DeviceDetectionManager.Exists)
            {
                DeviceDetectionManager.Enabled = scapi.CES.DeviceDetection.DeviceDetectionManager.IsEnabled;
                DeviceDetectionManager.Cache = scapi.Caching.CacheManager.GetAllCaches().First(x => x.Name.Equals("DeviceDetection"));
                CacheEntryRepeater.DataSource = DeviceDetectionManager.Cache.GetCacheKeys();
                CacheEntryRepeater.DataBind();
            }
#endif
        }
    }

    private static bool SitecoreCesDeviceDetectionExists()
    {
        try
        {
            System.Reflection.Assembly.Load("Sitecore.CES.DeviceDetection");

            return true;
        }
        catch(FileNotFoundException)
        {
            return false;
        }
    }

    private void SetBrowserCapabilitiesRepeater(string userAgent)
    {
        var fiftyOneDegreesService = new FiftyOneDegreesServiceFactory().Create(new Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers.HttpContextWrapper(HttpContext.Current));
        var detectedDevice = fiftyOneDegreesService.GetDetectedDevice(userAgent);

        if (detectedDevice != null)
        {
            BrowserCapabilitiesRepeater.DataSource = detectedDevice.DeviceProperties.Select(property => new {CapabilityKey = property, CapabilityValue = detectedDevice[property]});
        }

        BrowserCapabilitiesRepeater.DataBind();
    }
</script>


<!DOCTYPE html>
<html>
    <head>
        <title>Sitecore.51Degrees.CloudDeviceDetection Status</title>
        <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no">
        <link rel="stylesheet" href="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css" />
        <script type="text/javascript" src="//code.jquery.com/jquery-2.2.1.min.js"></script>
        <script type="text/javascript" src="//maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js"></script>
    </head>
    <body>
        <div class="container">
            <h1 style="word-wrap: break-word">Sitecore.51Degrees.CloudDeviceDetection Status</h1>
            <p>This admin page provides the status of the Sitecore.51Degrees.CloudDeviceDetection module.</p>
            <hr />
            <div class="panel-group" id="accordion" role="tablist" aria-multiselectable="true">
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingOne">
                        <h4 class="panel-title">
                            <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                              <strong>Detected Device Properties</strong>
                            </a>
                        </h4>
                    </div>
                    <div id="collapseOne" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                        <div class="panel-body">
                            <blockquote><%= HttpContext.Current.Request.UserAgent %></blockquote>
                            <table class="table table-striped table-hover">
                                <thead class="thead">
                                    <tr>
                                        <th>Browser Capability</th>
                                        <th>Value</th>
                                    </tr>
                                </thead>
                                <tbody>
                                <asp:Repeater runat="server" ID="BrowserCapabilitiesRepeater">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("CapabilityKey") %></td>
                                            <td><%# Eval("CapabilityValue") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="panel panel-default">
                    <div class="panel-heading" role="tab" id="headingTwo">
                        <h4 class="panel-title">
                            <a role="button" data-toggle="collapse" data-parent="#accordion" href="#collapseTwo" aria-expanded="true" aria-controls="collapseOne">
                              <strong>Device Detection Manager (Sitecore 8)</strong>
                            </a>
                        </h4>
                    </div>
                    <asp:Panel runat="server" ID="DeviceDetectionManagerDetailsPanel">
                        <div id="collapseTwo" class="panel-collapse collapse" role="tabpanel" aria-labelledby="headingOne">
                            <div class="panel-body">
                                <dl>
                                    <dt>Sitecore.CES.DeviceDetection.DeviceDetectionManager Type Exists: </dt>
                                    <dd><%= DeviceDetectionManager.Exists %></dd>
                                    <dt>Sitecore.CES.DeviceDetection.DeviceDetectionManager.IsEnabled: </dt>
                                    <dd><%= DeviceDetectionManager.Enabled %></dd>
                                    <dt>DeviceDetection Cache Count: </dt>
                                    <dd><%= DeviceDetectionManager.Cache.Count %></dd>
                                    <dt>DeviceDetection Cache Entries: </dt>
                                    <asp:Repeater runat="server" ID="CacheEntryRepeater">
                                        <HeaderTemplate><dd><ul></HeaderTemplate>
                                        <ItemTemplate>
                                            <li><a href="?useragent=<%# HttpUtility.UrlEncode(Container.DataItem.ToString()) %>"><%# Container.DataItem.ToString() %></a></li>
                                        </ItemTemplate>    
                                        <FooterTemplate></ul></dd></FooterTemplate>
                                    </asp:Repeater>
                                </dl>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </body>
</html>