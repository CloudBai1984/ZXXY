import frida
import sys
def onMessage(message,data):
    print(message)

device = frida.get_remote_device()
pid = device.spawn("com.palmmud.xyjclient")
session = device.attach(pid)
script = session.create_script("""

                 Module.enumerateImports("app_process32",{
                    onMatch:function(imp){
                        send(imp.name);
                                               
                    },
                    onComplete: function(){
                        send("Stop");
                    }
                 })

""")

script.on('message',onMessage)
script.load()
device.resume(pid)
sys.stdin.read()

session.detach()
