import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    redirectTo: '/livros',
    pathMatch: 'full'
  },
  {
    path: 'livros',
    loadComponent: () => import('./componentes/livros/livros.component').then(m => m.LivrosComponent)
  },
  {
    path: 'autores',
    loadComponent: () => import('./componentes/autores/autores.component').then(m => m.AutoresComponent)
  },
  {
    path: 'generos',
    loadComponent: () => import('./componentes/generos/generos.component').then(m => m.GenerosComponent)
  },
  {
    path: '**',
    redirectTo: '/livros'
  }
];
