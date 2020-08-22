using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BCore.Lib
{
    class MobinWebSocket
    {
        private ClientWebSocket ClientWS;
        Uri wsUri = new Uri("wss://push2v7.etadbir.com/lightstreamer");
        readonly ArraySegment<byte> WS_BUFFER;
        ArraySegment<byte> WS_SND_BUFFER;

        static string pattern = @"d\(1,1,2,'([0-1][0-9]|[2][0-3]):([0-5][0-9]):[0-5][0-9]'\);"; // d(1,1,2,'01:21:00')
        static string InnerPattern = @"([0-1][0-9]|[2][0-3]):([0-5][0-9]):[0-5][0-9]"; // 01:21:00
        Regex timeRegex = new Regex(pattern);
        Regex InnerTimeRegex = new Regex(InnerPattern);
        Match timeMatch;
        Match InnerTimeMatch;
        string wsRes = "";

        public MobinWebSocket()
        {
            ClientWS = new ClientWebSocket();
            /*ClientWS.Options.SetRequestHeader("Accept-Encoding", "gzip, deflate, br");
            ClientWS.Options.SetRequestHeader("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            ClientWS.Options.SetRequestHeader("Cache-Control", "no-cache");*/
            // ClientWS.Options.SetRequestHeader("Connection", "Upgrade");
            ClientWS.Options.SetRequestHeader("Host", "push2v7.etadbir.com");
            ClientWS.Options.SetRequestHeader("Origin", "https://silver.mobinsb.com");
            //ClientWS.Options.SetRequestHeader("Pragma", "no-cache");
            /*ClientWS.Options.SetRequestHeader("Sec-WebSocket-Extensions", "permessage-deflate; client_max_window_bits"); // ; client_max_window_bits"
            ClientWS.Options.SetRequestHeader("Sec-WebSocket-Key", Convert.ToBase64String(Encoding.UTF8.GetBytes("WebSocket rocks!")));
            ClientWS.Options.SetRequestHeader("Sec-WebSocket-Protocol", "js.lightstreamer.com");
            ClientWS.Options.SetRequestHeader("Sec-WebSocket-Version", "13");
            ClientWS.Options.SetRequestHeader("Upgrade", "websocket");*/
            ClientWS.Options.SetRequestHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36");
            ClientWS.Options.AddSubProtocol("js.lightstreamer.com");

            WS_BUFFER = new ArraySegment<byte>(new byte[4096]);
            WS_SND_BUFFER = new ArraySegment<byte>(new byte[4096]);
        }

        public async Task<bool> StartWebSocket(int phase, string session)
        {
            try
            {
                await ClientWS.ConnectAsync(wsUri, CancellationToken.None);
                if (ClientWS.State == WebSocketState.Open)
                {
                    string[] sends = new string[2]; // 8
                    sends[0] = $"bind_session{Environment.NewLine}LS_session={session}&LS_phase={phase + 2}&LS_cause=loop1&LS_container=lsc&";
                    sends[1] = $"control{Environment.NewLine}LS_mode=MERGE&LS_id=getclock&LS_schema=Key%20Type%20Value&LS_data_adapter=clock&LS_snapshot=false&LS_table=1&LS_req_phase=2&LS_win_phase=1&LS_op=add&LS_session={session}&";
                    //sends[2] = $"control{Environment.NewLine}LS_mode=MERGE&LS_id=irx6xtpi0006_lightrlc%20irxzxoci0006_lightrlc%20irx6xslc0006_lightrlc%20irx6xs300006_lightrlc%20irxyxtpi0026_lightrlc&LS_schema=ISIN%20SymbolTitle%20LastIndexValue%20IndexChanges%20PercentVariation%20DayOfEvent&LS_data_adapter=TadbirLightRLC&LS_snapshot=true&LS_table=2&LS_req_phase=3&LS_win_phase=1&LS_op=add&";
                    //sends[3] = $"control{Environment.NewLine}LS_mode=MERGE&LS_id=iro1gdir0001_lightrlc%20iro1bsdr0001_lightrlc%20iro1bank0001_lightrlc%20iro3dcaz0001_lightrlc%20iro1fold0001_lightrlc%20iro1zarm0001_lightrlc%20iro7aptp0001_lightrlc%20iro3urpz0001_lightrlc%20iro3mahz0001_lightrlc%20iro3zobz0001_lightrlc%20irr1kshj0101_lightrlc%20iro1kshj0001_lightrlc%20iro1beka0001_lightrlc%20iro1sadr0001_lightrlc%20iro3gedz0001_lightrlc%20iro1tsan0001_lightrlc&LS_schema=LastTradedPrice%20LastTradedPriceVarPercent%20TotalNumberOfSharesTraded%20ClosingPrice%20BestBuyLimitPrice_1%20BestSellLimitPrice_1%20BestBuyLimitQuantity_1%20BestSellLimitQuantity_1%20ClosingPriceVarPercent%20SymbolStateId%20InstrumentCode&LS_data_adapter=TadbirLightRLC&LS_snapshot=true&LS_table=3&LS_req_phase=4&LS_win_phase=1&LS_op=add&";
                    //sends[4] = $"control{Environment.NewLine}LS_mode=RAW&LS_id=777_msb01473118_lightrlc&LS_schema=text0%20conditionalalert0%20refresh%20logout&LS_data_adapter=TadbirLightPrivateGatewayAdapter&LS_snapshot=false&LS_table=4&LS_req_phase=5&LS_win_phase=1&LS_op=add&";
                    //sends[5] = $"control{Environment.NewLine}LS_mode=RAW&LS_id=777_lightrlc&LS_schema=text0%20conditionalalert0%20refresh%20logout&LS_data_adapter=TadbirLightPrivateGatewayAdapter&LS_snapshot=false&LS_table=5&LS_req_phase=6&LS_win_phase=1&LS_op=add&";
                    //sends[6] = $"control{Environment.NewLine}LS_mode=RAW&LS_id=777_msb01473118_lightrlc&LS_schema=orderstatev4&LS_data_adapter=TadbirLightPrivateGatewayAdapter&LS_snapshot=false&LS_table=6&LS_req_phase=7&LS_win_phase=1&LS_op=add&";
                    //sends[7] = $"control{Environment.NewLine}LS_mode=MERGE&LS_id=textmessage_lightrlc&LS_schema=TextMessage%20TextMessageTitle%20TextMessageDate%20TextMessageTime&LS_data_adapter=TadbirLightRLC&LS_snapshot=true&LS_table=7&LS_req_phase=8&LS_win_phase=1&LS_op=add&";

                    foreach (var s in sends)
                    {
                        WS_SND_BUFFER = new ArraySegment<byte>(Encoding.UTF8.GetBytes(s));
                        await ClientWS.SendAsync(WS_SND_BUFFER, WebSocketMessageType.Text, true, CancellationToken.None);
                    }
                    return true;
                }
                return false;
            }
            catch // (Exception ex)
            {
                // ILogger
                return false;
            }
        }

        private async void ReceiveData()
        {
            while (ClientWS.State == WebSocketState.Open)
            {
                try
                {
                    wsRes = "";
                    var result = await ClientWS.ReceiveAsync(WS_BUFFER, CancellationToken.None);
                    wsRes += Encoding.UTF8.GetString(WS_BUFFER.Array, 0, result.Count);
                    while (!result.EndOfMessage)
                    {
                        wsRes += Encoding.UTF8.GetString(WS_BUFFER.Array, 0, result.Count);
                    }

                    timeMatch = timeRegex.Match(wsRes);
                    if (timeMatch.Success)
                    {
                        InnerTimeMatch = InnerTimeRegex.Match(timeMatch.Value);
                        string[] timeValues = InnerTimeMatch.Value.Split(":");
                        ServerTime = new DateTime(
                            DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            int.Parse(timeValues[0]), int.Parse(timeValues[1]), int.Parse(timeValues[2]), ServerTime.Millisecond);
                    }

                    tb_ws_logs.AppendText(wsRes + Environment.NewLine);
                    tb_ws_logs.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    tb_ws_logs.AppendText(ex.Message + Environment.NewLine);
                    tb_ws_logs.ScrollToCaret();
                }
            }

        }
    }
}
