using Microsoft.Maui.Dispatching;
namespace MathGameguel
{
    public partial class MainPage : ContentPage
    {
        int iLB1 = 0;
        int iLB2 = 0;
        int iLB3 = 0;

        int dificuldade = 1;
        private int pontuacao = 0;
        int respostaCorreta = 0;
        private const int TotalQuestoes = 10;
        private int questoesRespondidas = 0;
        int iAcertCount = 0;
        int iErrorCount = 0;

        private readonly IDispatcherTimer timer;
        private int segundosRestantes = 30;

        Random rand = new Random();


        public MainPage()
        {
            InitializeComponent();

            timer = Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;


            GerarJogo();
            IniciarTimer();
        }
        private void AdicionarPontuacao()
        {
            int pontosBase = dificuldade switch
            {
                1 => 10,
                2 => 20,
                3 => 30,
                _ => 10
            };

            int bonusVelocidade = segundosRestantes;

            int pontosGanhos = pontosBase + bonusVelocidade;

            pontuacao += pontosGanhos;

            lbPontuacao.Text = $"Pontos: {pontuacao}";
        }
        private async void btOK_Clicked(object? sender, EventArgs e)
        {
            if (!int.TryParse(txR.Text, out int respostaUsuario))
            {
                await DisplayAlertAsync(
                    "ERRO",
                    "Digite um número válido!",
                    "OK"
                );

                return;
            }

            timer.Stop();
            btOK.IsEnabled = false;

            if (respostaUsuario == respostaCorreta)
            {
                iAcertCount++;

                AdicionarPontuacao();

                lbAcertou.Text = $"Acertos: {iAcertCount}";
                imR.Source = "win.png";
            }
            else
            {
                iErrorCount++;
                lbErrou.Text = $"Erros: {iErrorCount}";
                imR.Source = "loose.png";
            }

            await AvancarQuestao();
        }
        
        private void AtualizarProgresso()
        {
            pbProgresso.Progress =
                questoesRespondidas / (double)TotalQuestoes;

            if (questoesRespondidas >= TotalQuestoes)
            {
                lbQuestao.Text = "10 de 10 concluídas";
            }
            else
            {
                lbQuestao.Text =
                    $"Questão {questoesRespondidas + 1} de {TotalQuestoes}";
            }
        }
        private async Task AvancarQuestao()
        {
            questoesRespondidas++;
            AtualizarProgresso();

            await Task.Delay(2000);

            imR.Source = "question.png";

            if (questoesRespondidas >= TotalQuestoes)
            {
                await EncerrarPartida();
                return;
            }

            GerarJogo();
            IniciarTimer();

            btOK.IsEnabled = true;
        }
        private void txR_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textoDigitado = e.NewTextValue ?? "";

            string somenteNumeros = new string(
                textoDigitado.Where(char.IsDigit).ToArray()
            );

            if (textoDigitado != somenteNumeros)
            {
                txR.Text = somenteNumeros;
            }
        }

        private void IniciarTimer()
        {
            timer.Stop();
            segundosRestantes = 30;
            AtualizarTempoNaTela();

            timer.Start();
        
        }
        private void AtualizarTempoNaTela()
        {
            lbTempo.Text = $"Tempo: {segundosRestantes}s";

            if (segundosRestantes <= 5)
            {
                lbTempo.TextColor = Colors.Red;
            }
            else if (segundosRestantes <= 10)
            {
                lbTempo.TextColor = Colors.Orange;
            }
            else
            {
                lbTempo.TextColor = Colors.Green;
            }
        }
        private async Task EncerrarPartida()
        {
            timer.Stop();
            btOK.IsEnabled = false;

            int aproveitamento =
                iAcertCount * 100 / TotalQuestoes;

            string classificacao = aproveitamento switch
            {
                >= 90 => "Excelente",
                >= 70 => "Bom",
                >= 50 => "Regular",
                _ => "Precisa praticar"
            };

            await DisplayAlertAsync(
                "Resultado final",
                $"Acertos: {iAcertCount}\n" +
                $"Erros: {iErrorCount}\n" +
                $"Aproveitamento: {aproveitamento}%\n" +
                $"Pontuação final: {pontuacao}\n" +
                $"Classificação: {classificacao}",
                "Jogar novamente"
            );

            ReiniciarPartida();
        }
        private void ReiniciarPartida()
        {
            questoesRespondidas = 0;
            iAcertCount = 0;
            iErrorCount = 0;
            pontuacao = 0;

            lbAcertou.Text = "Acertos: 0";
            lbErrou.Text = "Erros: 0";
            lbPontuacao.Text = "Pontos: 0";
            imR.Source = "question.png";

            AtualizarProgresso();
            GerarJogo();
            IniciarTimer();

            btOK.IsEnabled = true;
        }
        private async void Timer_Tick(object? sender, EventArgs e)
        {
            segundosRestantes--;
            AtualizarTempoNaTela();

            if (segundosRestantes <= 0)
            {
                timer.Stop();
                btOK.IsEnabled = false;

                iErrorCount++;
                lbErrou.Text = $"Erros: {iErrorCount}";
                imR.Source = "loose.png";

                await AvancarQuestao();
            }
        }
        private void GerarJogo()
        {
            int limite = dificuldade switch
            {
                1 => 10,
                2 => 50,
                3 => 100,
                _ => 10
            };

            iLB1 = rand.Next(1, limite + 1);
            iLB2 = rand.Next(1, 5);
            iLB3 = rand.Next(1, limite + 1);

            switch (iLB2)
            {
                case 1:
                    respostaCorreta = iLB1 + iLB3;
                    lb2.Text = "+";
                    break;

                case 2:
                    if (iLB1 < iLB3)
                    {
                        (iLB1, iLB3) = (iLB3, iLB1);
                    }

                    respostaCorreta = iLB1 - iLB3;
                    lb2.Text = "-";
                    break;

                case 3:
                    respostaCorreta = iLB1 * iLB3;
                    lb2.Text = "×";
                    break;

                case 4:
                    iLB3 = rand.Next(1, limite + 1);

                    int maiorQuociente = limite / iLB3;
                    int quociente = rand.Next(1, maiorQuociente + 1);

                    iLB1 = iLB3 * quociente;
                    respostaCorreta = quociente;
                    lb2.Text = "÷";
                    break;
            }

            lb1.Text = iLB1.ToString();
            lb3.Text = iLB3.ToString();

            txR.Text = "";
        }

        private void Facil_Clicked(object sender, EventArgs e)
        {
            dificuldade = 1;
            GerarJogo();
            IniciarTimer();
        }

        private void medio_Clicked(object sender, EventArgs e)
        {
            dificuldade = 2;
            GerarJogo();
            IniciarTimer();
        }

        private void dificl_Clicked(object sender, EventArgs e)
        {
            dificuldade = 3;
            GerarJogo();
            IniciarTimer();
        }
    }
}