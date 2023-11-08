namespace PrestitiBancari
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
        }
        class Banca
        {
            private string nome;
            private List<Cliente> clienti;

            //Proprietà dei dati get e set
            public string Nome { get; set; }
            public List<Cliente> Clienti { get; set; }

            //Costruttore
            public Banca(string nome, List<Cliente> clienti)
            {
                this.nome = nome;
                this.clienti = clienti;
            }

            public void AddClient(Cliente cliente)
            {

            }
            public void RemoveClient(string )
            {

            }
            public Cliente SearchClient()
            {
                return ;
            }
            public void AddPrestito()
            {

            }
            public List<PrestitoSemplice> SearchPrestiti()
            {
                return ;
            }
            public double TotalePrestiti()
            {

            }
        }

        class Cliente
        {
            private string nome, cognome, codiceFiscale;
            private double stipendio;
            private List<PrestitoSemplice> prestiti;

            //Proprietà dei dati get e set
            public string Nome { get; set; }
            public string Cognome { get; set; }
            public string CodiceFiscale { get; set; }
            public string Stipendio { get; set; }
            public List<PrestitoSemplice> Prestiti { get; set; }

            //Costruttore
            public Cliente(string nome, string cognome, string codiceFiscale, double stipendio)
            {
                this.nome = nome;
                this.cognome = cognome;
                this.codiceFiscale = codiceFiscale;
                this.stipendio = stipendio;

            }

            public void StampaCliente()
            {

            }

        }
        class PrestitoSemplice : Cliente 
        {
            double capitale, interesse, rata, montante, durata;
            DateTime dataInizio, dataFine;

            //Proprietà dei dati get e set
            public string Capitale { get; set; }
            public string Interesse { get; set; }
            public string Rata { get; set; }
            public string Montante { get; set; }
            public string Durata { get; set; }
            public DateTime DataInizio { get; set; }
            public DateTime DataFine { get; set; }

            //Costruttore
            public PrestitoSemplice(string nome, string cognome, string codiceFiscale, double stipendio, double capitale, double interesse,
                DateTime dataInizio, DateTime dataFine, double rata, double montante, double durata)
                : base(nome, cognome, codiceFiscale, stipendio) //Chiama il costruttore della classe base
            {
                this.capitale = capitale;
                this.interesse = interesse;
                this.dataInizio = dataInizio;
                this.dataFine = dataFine;
                this.rata = rata;
                this.montante = montante;
                this.durata = durata;
            }

            public void StampaPrestito()
            {

            }
        }
        /*class PrestitoComposto : PrestitoSemplice
        {
            //montante --> override
        }*/
    }
}