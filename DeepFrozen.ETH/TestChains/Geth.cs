using DeepCore.IO;
using DeepFrozen.ETH.Contracts.Truffle;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace DeepFrozen.ETH.TestChains
{
    public class Geth
    {
        public static FileInfo DefaultGenesisConfig { get; private set; }
        public static bool TryFindGeth(string root, out Geth geth)
        {
            geth = null;
            if (CFiles.TryFindParentFile(root, Path.Combine(root, "geth-clique-windows", "geth.exe"), out var _geth))
            {
                DefaultGenesisConfig = new FileInfo(Path.Combine(_geth, "genesis_clique.json"));
                geth = new Geth(new FileInfo(_geth));
                return true;
            }
            return false;
        }

        //------------------------------------------------------------------------------------------------------------------------
        public FileInfo GethFile { get; }
        public DirectoryInfo WorkDirectory { get; set; }
        public string DataDir { get; set; } = "devChain";
        public string RpcCorsDomain { get; set; } = "\"*\"";
        public string RpcAPI { get; set; } = "\"eth,web3,personal,net,miner,admin,debug\"";
        public string Unlock { get; set; } = "0x12890d2cce102216644c59daE5baed380d84830c";
        public string PasswordFile { get; set; } = "password";
        public Geth(FileInfo geth)
        {
            GethFile = geth;
            WorkDirectory = geth.Directory; 
        }

        public Process Init(string genesis_json = "genesis_clique.json")
        {
            var exe = new ProcessStartInfo();
            exe.WorkingDirectory = Path.GetDirectoryName(WorkDirectory.FullName);
            exe.FileName = GethFile.Name;
            exe.Arguments = $"--datadir={DataDir} init {genesis_json}";
            return Process.Start(exe);
        }

        public Process Run( )
        {
            var exe = new ProcessStartInfo();
            exe.WorkingDirectory = Path.GetDirectoryName(WorkDirectory.FullName);
            exe.FileName = GethFile.Name;
            exe.Arguments = $"--nodiscover --rpc --datadir={DataDir}  --rpccorsdomain {RpcCorsDomain} --mine --rpcapi {RpcAPI} --unlock {Unlock} --password {PasswordFile} --verbosity 0 console";
            return Process.Start(exe);
        }

//         public void MainLoop( ) 
//         {
//             var exe = new ProcessStartInfo();
//             exe.WorkingDirectory = Path.GetDirectoryName(WorkDirectory.FullName);
//             exe.FileName = "cmd";
//             exe.UseShellExecute = false;
//             exe.RedirectStandardInput = true;
//             //log.Info($"Shutdown... : {Redis.StartInfo.FileName}");
//             var my = Process.Start(exe);
//             my.StandardInput.WriteLine($"cd {WorkDirectory.FullName}");
//             my.StandardInput.WriteLine($"truffle compile");
//             my.StandardInput.WriteLine("exit");
//             my.StandardInput.Flush();
//             my.WaitForExit();
//         }

    }
}
