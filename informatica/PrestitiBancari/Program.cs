namespace PrestitiBancari
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Banca banca1 = new Banca();

            Cliente cliente0 = new Cliente("Giulio", "Zangheri", "ZNGGLI06R30C573N", 1700.0);
        }

        class Banca
        {
            private string nome;
            private List<Cliente> clienti = new List<Cliente>();

            //proprietà Get Set
            public string Nome { get; set; }
            public List<Cliente> Clienti { get; set; }

            //Costruttori
            public Banca(string nome, List<Cliente> clienti)
            {
                this.nome = nome;
                this.clienti = clienti;
            }
            public Banca(string nome)
            {
                this.nome = nome;
                this.clienti =  new List<Cliente>();
            }
            public Banca()
            {
                this.nome = "";
                this.clienti = new List<Cliente>();
            }

            public void AddCliente(Cliente cliente)
            {
                clienti.Add(cliente);
            }
            public void RemoveCliente(string codiceFiscale)
            {
                clienti.Remove(SearchCliente(codiceFiscale));
            }
            public Cliente SearchCliente(string codiceFiscale)
            {
                foreach (Cliente cliente in clienti)
                {
                    if (cliente.CodiceFiscale == codiceFiscale)
                    {
                        return cliente;
                    }
                }
                throw new Exception("Cliente non trovato");
            }
            public void AddPrestito(PrestitoSemplice prestito, string codiceFiscale)
            {
                SearchCliente(codiceFiscale).Prestiti.Add(prestito);
            }
            public List<PrestitoSemplice> SearchPrestiti(string codiceFiscale)
            {
                return SearchCliente(codiceFiscale).Prestiti;
            }
            public double TotalePrestiti(string codiceFiscale)
            {
                double sum = 0.0;
                foreach (PrestitoSemplice prestito in SearchPrestiti(codiceFiscale))
                {
                    sum += prestito.Capitale;
                }
                return sum;
            }

        }

        class Cliente
        {
            private string nome, cognome, codiceFiscale;
            private double stipendio;
            private List<PrestitoSemplice> prestiti = new List<PrestitoSemplice>();

            public string Nome { get; set; }
            public string Cognome { get; set; }
            public string CodiceFiscale { get; set; }
            public double Stipendio { get; set; }
            public List<PrestitoSemplice> Prestiti { get; set; }

            //Costruttore
            public Cliente(string nome, string cognome, string codiceFiscale, double stipendio, List<PrestitoSemplice> prestiti)
            {
                this.nome = nome;
                this.cognome= cognome;
                this.codiceFiscale = codiceFiscale;
                this.stipendio = stipendio;
                this.prestiti = prestiti;
            }
            public Cliente(string nome, string cognome, string codiceFiscale, double stipendio)
            {
                this.nome = nome;
                this.cognome = cognome;
                this.codiceFiscale = codiceFiscale;
                this.stipendio = stipendio;
                this.prestiti = new List<PrestitoSemplice>();
            }

            public void StampaCliente()
            {
                Console.WriteLine($"{nome} {cognome} - {codiceFiscale} - {stipendio}/m");
                foreach (PrestitoSemplice prestito in prestiti)
                {
                    Console.WriteLine($"{prestito.DataInizio} -> {prestito.DataFine} | {prestito.Capitale}");
                }
            }
        }

        class PrestitoSemplice
        {
            private string codiceFiscale;
            private double capitale, interesse, rata, montante, durata;
            private DateTime dataInizio, dataFine;

            public string CodiceFiscale { get; set; }
            public double Capitale { get; set; }
            public double Interesse { get; set; }
            public double Rata { get; set; }
            public double Montante { get; set; }
            public double Durata { get; set; }
            public DateTime DataInizio { get; set; }
            public DateTime DataFine { get; set; }

            //Costruttore
            public PrestitoSemplice(string codiceFiscale, double capitale, double interesse, DateTime dataInizio, DateTime dataFine, double rata)
            {
                this.codiceFiscale = codiceFiscale;
                this.capitale = capitale;
                this.interesse = interesse;
                this.dataInizio = dataInizio;
                this.dataFine = dataFine;
                this.rata = rata;
                this.durata = CalcolaDurata(dataInizio, dataFine);
                this.montante = capitale*(1+durata*interesse);
                
            }

            public void StampaPrestito()
            {
                Console.WriteLine( "------------------------------------------");
                Console.WriteLine($"{DataInizio} -> {DataFine} | {Capitale}€ |");
                Console.WriteLine($"durata: {durata}");
                Console.WriteLine($"interesse: {interesse}€");
                Console.WriteLine($"rate: {rata}€");
                Console.WriteLine($"montante: {montante}");
                Console.WriteLine( "------------------------------------------");
            }

            private double CalcolaDurata(DateTime dataInizio, DateTime dataFine)
            {
                TimeSpan diff = dataFine.Subtract(dataInizio);
                return diff.TotalDays;
            }
        }

        class PrestitoComposto : PrestitoSemplice //dipende dalla classe prestito semplice
        {
            //Costruttore
            public PrestitoComposto(string codiceFiscale, double capitale, double interesse, DateTime dataInizio, DateTime dataFine, double rata, double durata)
                : base(codiceFiscale, capitale, interesse, dataInizio, dataFine, rata)
            {
            }
            
            //montante --> override
            //durata
        }
    }
}