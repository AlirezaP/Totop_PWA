using BlazorApp2.Client.Pages.Components;
using BlazorApp2.Client.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorApp2.Client.Pages
{
    public partial class Index
    {
        private IDataBase _db;

        private static List<Data> tmp = new List<Data>();
        private List<TotpService> _totpList = new List<TotpService>();

        //----------------------------------------------------------------
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _db = new BlazorApp2.Client.Services.DataBase(DbFactory, IJS, "secret");
                LoadData();

            }

            base.OnAfterRender(firstRender);
        }

        //----------------------------------------------------------------

        private async void LoadData()
        {
            var allItems = await _db.GetAllTotp();

            if (allItems is null || allItems.Length <= 0)
            {
                return;
            }

            int counter = 1;
            string preID = "id";
            foreach (var item in allItems)
            {
                if(item.KeySec is null)
                {
                    continue;
                }

                var current = tmp.Where(p => p.Title == item.LoginCode).FirstOrDefault();
                if (current is not null)
                {
                    current.Timer = current.TotpM.Remaining();
                    continue;
                }

                var tmpTotp = TotpService.Create(item.KeySec, item.LoginCode, item.Time, item.Step);
                _totpList.Add(tmpTotp);

                tmp.Add(new Data
                {
                    Title = item.LoginCode,
                    Digit = tmpTotp.GetOtp(),
                    Lable = item.LoginCode,
                    IsExpanded = false,
                    Timer = tmpTotp.Remaining(),
                    ElementID = $"{preID}{counter}",
                    TotpM = tmpTotp
                });
            }


            StateHasChanged();
        }

        private void GenerateNewOtp(object obj)
        {
            var item = (obj as LoadingCircle);

            var currentOtp = _totpList.Where(p => p.LoginCode == item.UID).FirstOrDefault();
            var currettmp = tmp.Where(p => p.Title == item.UID).FirstOrDefault();
            currettmp.Digit = currentOtp.GetOtp();
            item.TimerValue = currentOtp.Remaining();
            item.SetParam(currentOtp.Remaining());
            StateHasChanged();
        }

        //------------------------------------------

        public class Data
        {
            private double timer;
            public string Title { get; set; }
            public string Lable { get; set; }
            public double Timer { get
                {
                    return timer;
                }
                set
                {
                    if (value <= 0)
                    {
                        Digit = TotpM.GetOtp();
                        timer = TotpM.Remaining();
                    }
                    else
                    {
                        timer = value;
                    }
                }
            }
            public string Digit { get; set; }
            public bool IsExpanded { get; set; }
            public string ElementID { get; set; }

            public TotpService TotpM { get; set; }
        }

    }
}
