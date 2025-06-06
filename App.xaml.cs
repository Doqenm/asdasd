﻿using Microsoft.Maui.Controls;
using Managing;
using Managing.Models;

namespace Managing
{
    public partial class App : Application
    {
        public static User? CurrentUser { get; private set; }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
            // Não chamar a verificação de autenticação aqui, porque Shell.Current ainda não está definido.
            // Em vez disso, utilize o evento AppShell.OnAppearing ou AppShell.OnNavigated, ou trate-o no construtor do AppShell após InitializeComponent.
        }

        private async void VerificarAutenticacao()
        {
            if (CurrentUser == null)
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }

        public static void DefinirUtilizador(User user)
        {
            CurrentUser = user;
            // Atualizar visibilidade do menu/permissões aqui se necessário
        }

        public static void TerminarSessao()
        {
            CurrentUser = null;
            Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
