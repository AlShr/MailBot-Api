Удаленный доступ:
1. перейти в %userprofile%\documents\iisexpress\config\applicationhost.config
2 найти 
<site name="BotWcfServiceIIS" id="19">
     <application path="/" applicationPool="Clr4IntegratedAppPool">
      	   <virtualDirectory path="/" physicalPath="C:\Users\alena\Documents\Visual Studio 2013\Projects\Mail Bot\BotWcfServiceIIS\BotWcfServiceIIS" />
      </application>
      <bindings>
           <binding protocol="http" bindingInformation="*:9611:localhost" />                
      </bindings>
</site>

добавить 

<binding protocol="http" bindingInformation="*:9611:192.168.1.102" /> 

3.  настроить IIS Express принимать входящие соединения на порт 9608 
в командной строке  выполнить 
> netsh http add urlacl url=http://192.168.1.102:9611/ user=все
4. настройка брандмауэра Windows, чтобы разрешить внешний трафик на порту 9611
в командной строке  выполнить 
> netsh advfirewall firewall add rule name="IISExpressXamarin" dir=in protocol=tcp localport=9611 profile=private remoteip=localsubnet action=allow

5. Сгенерировать прокси-сервис с помощью в
C:\Program Files (x86)\Microsoft SDKs\Silverlight\v5.0\Tools\SLsvcUtil.exe

SLsvcUtil.exe /noConfig http://localhost:9611/BotMailService.svc?wsdl
6.добавить сгенерированные файлы в клиент