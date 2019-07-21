﻿using System;
using System.Collections.Generic;
using TheAwesomeTextAdventure.Domain.Characters;
using TheAwesomeTextAdventure.Infrastructure.Writers.Abstractions;
using TheAwesomeTextAdventure.Processor.Abstractions;
using TheAwesomeTextAdventure.Services.Abstractions;

namespace TheAwesomeTextAdventure.Processor
{
    public class AdventureInitializer : BaseProcessor, IAdventureInitializer
    {
        public IAdventureProcessor AdventureProcessor { get; }

        public IPlayerHandler PlayerHandler { get; }

        private Dictionary<string, Action> _actionList
            => new Dictionary<string, Action>
            {
                { "NOVO JOGO", () => StartAdventure(null) },
                { "CARREGAR JOGO SALVO", () => StartAdventure(LoadPlayer())},
                { "SAIR", Exit },
            };

        public AdventureInitializer(
            IPlayerWriter playerWriter,
            IAdventureProcessor adventureProcessor,
            IPlayerHandler playerHandler) : base(playerWriter)
        {
            AdventureProcessor = adventureProcessor ?? throw new ArgumentNullException(nameof(adventureProcessor));
            PlayerHandler = playerHandler ?? throw new ArgumentNullException(nameof(playerHandler));
        }

        public void StartGame()
        {
            ReadAction(_actionList);
        }

        private void StartAdventure(Player player)
        {
            if (player == null)
                player = PlayerHandler.CreatePlayer();

            AdventureProcessor.StartAdventure(player);
        }

        private Player LoadPlayer()
            => PlayerHandler.LoadPlayer();
    }
}
