		public void getWinSCPFile(out string exception, out bool blnSuccess)
        {
            exception = string.Empty;
            blnSuccess = false;

            try
            {
                string local_path = string.Empty;
                string temp_path = string.Empty;
                string today = DateTime.Today.ToString("yyMMdd");
                SessionOptions sessionOptions = new SessionOptions
                {

                    Protocol = Protocol.Sftp,
                    HostName = "example.com",
                    UserName = "jkljjop",
                };

                Thread.Sleep(1000);

                string private_key_path = string.Empty;
                getPathFromDB("private key", out private_key_path, out exception, out blnSuccess);
                if (!blnSuccess)
                {
                    return;
                }
                sessionOptions.SshPrivateKeyPath = private_key_path;

                sessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;

                using (Session session = new Session())
                {
                    try
                    {
                        session.ExecutablePath = @"C:\Program Files (x86)\WinSCP\WinSCP.exe";
                        session.Open(sessionOptions);
                        Thread.Sleep(500);
                        string remotepath = string.Empty;
                        getPathFromDB("MEMO", out remotepath, out exception, out blnSuccess);
                        if (!blnSuccess)
                        {
                            return;
                        }

                        Thread.Sleep(500);

                        string temp_remote = remotepath;

                        temp_path = local_path;

                        getPathFromDB("Memo - Local", out local_path, out exception, out blnSuccess);
                        if (!blnSuccess)
                        {
                            return;
                        }

                        Thread.Sleep(500);

                        temp_path = local_path;

                        createFolder(DateTime.Now, local_path, out local_path, out exception, out blnSuccess);
                        if (!blnSuccess)
                        {
                            return;
                        }
 
						Thread.Sleep(1000);

                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Binary;
                        TransferOperationResult transferResult;
                        remotepath = remotepath + "TrdDetailRep_Memo_All." + today + "_0300.rpt";
                        if (session.FileExists(remotepath))
                        {
                            if (!File.Exists(local_path + "TrdDetailRep_Memo_All." + today + "_0300.txt") && !File.Exists(local_path + "TrdDetailRep_Memo_All." + today + "_0300.rpt"))
                            {
                                transferResult = session.GetFiles(remotepath, local_path, false, transferOptions);
                                transferResult.Check();
                            }

                            Thread.Sleep(2000);
                        }
                        else
                        {
                            DateTime temp_date = DateTime.Now.AddDays(-1);

                            createFolder(temp_date, temp_path, out local_path, out exception, out blnSuccess);
                            if (!blnSuccess)
                            {
                                return;
                            }

                            remotepath = temp_remote + "TrdDetailRep_Memo_All." + temp_date.ToString("yyMMdd") + "_0300.rpt";
                            if (session.FileExists(remotepath))
                            {
                                if (!File.Exists(local_path + "TrdDetailRep_Memo_All." + temp_date.ToString("yyMMdd") + "_0300.txt") && !File.Exists(local_path + "TrdDetailRep_Memo_All." + temp_date.ToString("yyMMdd") + "_0300.rpt"))
                                {
                                    transferResult = session.GetFiles(remotepath, local_path, false, transferOptions);
                                    transferResult.Check();
                                }

								Thread.Sleep(2000);
                                today = temp_date.ToString("yyMMdd");
                            }
                            else
                            {
                                temp_date = temp_date.AddDays(-1);

                                createFolder(temp_date, temp_path, out local_path, out exception, out blnSuccess);
                                if (!blnSuccess)
                                {
                                    return;
                                }

                                remotepath = temp_remote + "TrdDetailRep_Memo_All." + temp_date.ToString("yyMMdd") + "_0300.rpt";
                                if (session.FileExists(remotepath))
                                {
                                    if (!File.Exists(local_path + "TrdDetailRep_Memo_All." + temp_date.ToString("yyMMdd") + "_0300.txt") && !File.Exists(local_path + "TrdDetailRep_Memo_All." + temp_date.ToString("yyMMdd") + "_0300.rpt"))
                                    {
                                        transferResult = session.GetFiles(remotepath, local_path, false, transferOptions);
                                        transferResult.Check();
                                    }
									Thread.Sleep(2000);
                                    today = temp_date.ToString("yyMMdd");
                                }
                                else
                                {
                                    exception = "File for today's date not found in FEDS MEMO exe";
                                    return;
                                }
                            }
                            
                        }
                        Thread.Sleep(2000);
                    }
                    catch (System.Exception ex)
                    {
                        exception = ex.Message;
						MessageBox.Show(exception);
                        return;
                    }
                }

                // Renaming file

                foreach (string file in Directory.GetFiles(local_path))
                {
                    if (file.Contains(today))
                    {
                        FileInfo fi = new FileInfo(file);
                        if (!File.Exists(file.Replace(".rpt", ".txt")))
                        {
                            File.Move(file, file.Replace(".rpt", ".txt"));
                        }
                        
                        break;
                    }
                }

                blnSuccess = true;
            }
            catch (System.Exception ex)
            {
                exception = ex.Message;
				MessageBox.Show(exception);
            }
       
        }
        
        
