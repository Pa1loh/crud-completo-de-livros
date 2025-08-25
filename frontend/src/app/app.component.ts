import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, CommonModule],
  template: `
    <div class="app-container">
      <header class="app-header">
        <h1>Sistema de Gerenciamento de Livros</h1>
        <nav>
          <a routerLink="/livros" routerLinkActive="active">Livros</a>
          <a routerLink="/autores" routerLinkActive="active">Autores</a>
          <a routerLink="/generos" routerLinkActive="active">Gêneros</a>
        </nav>
      </header>
      <main class="app-main">
        <router-outlet></router-outlet>
      </main>
    </div>
  `,
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'Sistema de Gerenciamento de Livros';
}
