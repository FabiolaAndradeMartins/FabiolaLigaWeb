using LigaWeb.Data.Entities;

namespace LigaWeb.Models
{
    public class GamesIndexViewModel
    {
        public IEnumerable<Game> Games { get; set; }

		// Dados para o Gráfico de Barras
		public string BarChartData { get; set; }
    }
}
