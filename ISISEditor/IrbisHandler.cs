using System;
using ManagedClient;
using System.Windows.Forms;

namespace ISISEditor
{
    internal class IrbisHandler
    {
        internal Logging logging;
        internal bool connected = false;
        private ManagedClient64 client = new ManagedClient64();
        private IrbisRecord recordFrom = new IrbisRecord();
        private IrbisRecord recordTo = new IrbisRecord();
        private string _recordIndex;


        internal void Connect(string database, string login, string password)
        {
            try
            {
                if (connected)
                {
                    Disconnect();
                }
                client.ParseConnectionString("host=127.0.0.1;port=8888; user=" + login + ";password=" + password + ";");
                //client.ParseConnectionString("host=194.169.10.3;port=8888; user=" + login + ";password=" + password + ";");
                client.Connect();
                client.PushDatabase(database);
                connected = true;
                MessageBox.Show("Connected!");
            }
            catch (Exception ex)
            {
                logging.WriteLine("ERROR DURING CONNECTION!");
                logging.WriteLine(ex.StackTrace);
                logging.WriteLine(ex.ToString());
                MessageBox.Show("Error!");
            }

        }

        internal void Disconnect()
        {
            try
            {
                client.Disconnect();
            }
            catch (Exception ex)
            {
                logging.WriteLine("ERROR DURING DISCONNECTION!");
                logging.WriteLine(ex.StackTrace);
                logging.WriteLine(ex.ToString());
            }
        }

        internal void CopyRecord(string fromMfn, string toMfn, string recordIndex)
        {
            _recordIndex = "(" + recordIndex + ") ";
            recordFrom = client.ReadRecord(Int32.Parse(fromMfn));
            recordTo = client.ReadRecord(Int32.Parse(toMfn));
            try
            {
                RecordField fiedl922 = new RecordField("922");
                AddSubfield(fiedl922, 'C', "200", 'A');
                AddSubfield(fiedl922, 'F', "700", 'A');
                AddSubfield(fiedl922, '?', "700", 'G');
                AddSubfield(fiedl922, '1', "700", '1');
                AddSubfield(fiedl922, 'A', "700", 'C');
                recordTo.Fields.Add(fiedl922);

                CopyField("300");
                CopyField("331");
                CopySubField("215", 'A');
                CopySubField("316", 'A');


                foreach (RecordField field210 in recordFrom.Fields.GetField("210"))
                {
                    if (!field210.ToString().Contains("^5"))
                    {
                        _recordIndex = _recordIndex.Replace(" ", "");
                    }

                    var field = new RecordField("210");
                    var sub2105 = new SubField('5', _recordIndex + field210.GetFirstSubFieldText('5'));
                    var sub210d = new SubField('D', field210.GetFirstSubFieldText('d'));
                    field.SubFields.Add(sub210d);
                    field.SubFields.Add(sub2105);
                    logging.WriteLine(field.Text);
                    recordTo.Fields.Add(field);
                    logging.WriteLine(recordTo.FM("210"));

                }



                client.WriteRecord
                            (
                                recordTo,
                                false,
                                true
                            );

            }
            catch (Exception ex)
            {
                logging.WriteLine("ERROR DURING COPYING RECORD! ");
                logging.WriteLine(ex.StackTrace);
                logging.WriteLine(ex.ToString());
            }

        }

        private void CopyField(string tagFrom)
        {
            if (!String.IsNullOrEmpty(recordFrom.FM(tagFrom)))
            {
                foreach (string field in recordFrom.FMA(tagFrom))
                {
                    recordTo.Fields.Add(new RecordField(tagFrom, _recordIndex + field));
                }
            }
        }

        private void CopyField(string tagFrom, string tagTo)
        {
            if (!String.IsNullOrEmpty(recordFrom.FM(tagFrom)))
            {
                foreach (string field in recordFrom.FMA(tagFrom))
                {
                    recordTo.Fields.Add(new RecordField(tagTo, _recordIndex + field));
                }
            }
        }

        private void CopySubField(string tagFrom, char codeFrom)
        {
            if (!String.IsNullOrEmpty(recordFrom.FM(tagFrom, codeFrom)))
            {
                foreach (string field in recordFrom.FMA(tagFrom, codeFrom))
                {
                    recordTo.AddField(tagFrom, codeFrom, _recordIndex + field);
                }
            }
        }

        private void CopySubField(string tagFrom, char codeFrom, string tagTo, char codeTo)
        {
            if (!String.IsNullOrEmpty(recordFrom.FM(tagFrom, codeFrom)))
            {
                foreach (string field in recordFrom.FMA(tagFrom, codeFrom))
                {
                    recordTo.AddField(tagTo, codeTo, _recordIndex + field);
                }
            }
        }

        private void AddSubfield(RecordField field, char subFieldCode, string tagFrom, char codeFrom)
        {
            if (!String.IsNullOrEmpty(recordFrom.FM(tagFrom, codeFrom)))
            {
                SubField subField = new SubField(subFieldCode, recordFrom.FM(tagFrom, codeFrom));
                field.SubFields.Add(subField);
            }
        }
    }
}