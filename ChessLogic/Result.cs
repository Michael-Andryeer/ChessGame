namespace ChessLogic
{
    // Define um namespace chamado ChessLogic para encapsular as classes relacionadas à lógica do xadrez
    public class Result
    {
        // Declaração das propriedades Winner e Reason, que armazenam o vencedor e o motivo do resultado do jogo
        public Player Winner { get; } // Propriedade para armazenar o vencedor do jogo
        public EndReason Reason { get; } // Propriedade para armazenar o motivo do resultado do jogo

        // Construtor da classe Result, que inicializa as propriedades Winner e Reason
        public Result(Player winner, EndReason reason)
        {
            Winner = winner; // Atribui o vencedor passado como argumento à propriedade Winner
            Reason = reason; // Atribui o motivo passado como argumento à propriedade Reason
        }

        // Método estático chamado Win, que cria e retorna uma instância de Result indicando que um jogador venceu o jogo
        public static Result Win(Player winner)
        {
            return new Result(winner, EndReason.Checkmate); // Cria e retorna uma instância de Result com o vencedor e o motivo de vitória (xeque-mate)
        }

        // Método estático chamado Draw, que cria e retorna uma instância de Result indicando um empate no jogo
        public static Result Draw(EndReason reason)
        {
            return new Result(Player.None, reason); // Cria e retorna uma instância de Result com nenhum vencedor (empate) e o motivo fornecido
        }
    }
}
