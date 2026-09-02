namespace MathGameguel
{
    public partial class MainPage : ContentPage
    {
        int iLB1 = 0;
        int iLB2 = 0;
        int iLB3 = 0;

        int dificuldade = 1;

        float fR = 0.0f;

        int iAcertCount = 0;
        int iErrorCount = 0;

        Random rand = new Random();


        public MainPage()
        {
            InitializeComponent();

            GerarJogo();
        }

        private async void btOK_Clicked(object sender, EventArgs e)
        {
            float fResult = 0.0f;

            try
            {
                fResult = Convert.ToSingle(txR.Text);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("ERRO", "Digite um número válido!", "OK");
                return;
            }

            //testa se o valor digitado pelo usuário acertou o resultado da equação
            if (fResult == fR)
            {
                iAcertCount++;
                lbAcertou.Text = "Acertos: " + Convert.ToString(iAcertCount);
                imR.Source = "win.png";
            }
            else
            {
                iErrorCount++;
                imR.Source = "loose.png";
                lbErrou.Text = $"Errou: {iErrorCount}";
            }

            await Task.Delay(2000);
            imR.Source = "question.png";

            GerarJogo();
        }



        private void GerarJogo()
        {
            int limite = dificuldade
                switch
            { 
                    1=>10,
                    2=>50,
                    3=>100,
                     _ =>10
                };

            iLB1 = rand.Next(1,limite + 1);
            iLB2 = rand.Next(1, 5);
            iLB3 = rand.Next(1, limite + 1);

            lb1.Text = Convert.ToString(iLB1);
            lb3.Text = Convert.ToString(iLB3);

            switch (iLB2)
            {
                case 1:
                    fR = (iLB1 + iLB3);
                    lb2.Text = "+";
                    break;
                case 2:
                    fR = (iLB1 - iLB3);
                    lb2.Text = "-";
                    break;
                case 3:
                    fR = (iLB1 * iLB3);
                    lb2.Text = "×";
                    break;
                case 4:
                    fR = (iLB1 / iLB3);
                    lb2.Text = "÷";
                    break;
            }

            txR.Text = "";
        } //fim do GerarJogo()

        private void Facil_Clicked(object sender, EventArgs e)
        {
            dificuldade = 1;
            GerarJogo();
        }

        private void medio_Clicked(object sender, EventArgs e)
        {
            dificuldade = 2;
            GerarJogo();
        }

        private void dificl_Clicked(object sender, EventArgs e)
        {
            dificuldade = 3;
            GerarJogo();
        }
    }
}