import { NgModule } from '@angular/core';
import { Routes, RouterModule, NoPreloading } from '@angular/router';


const routes: Routes = [
  {
    path: 'category',
    loadChildren: () => import('./categories/categories.module')
    .then(m => m.CategoriesModule)
  },
  {
    path: 'product',
    loadChildren: () => import('./products/products.module')
    .then(m => m.ProductsModule)
  },
  {
    path: 'feature',
    loadChildren: () => import('./features/features.module')
    .then(m => m.FeaturesModule)
  },
  {
    path: 'auth',
    loadChildren: () => import('./auth/auth.module')
    .then(m => m.AuthModule)
  },
  { path: '', redirectTo: '/category', pathMatch: 'full' },
  { path: '**', redirectTo: '/auth'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {preloadingStrategy: NoPreloading} )],
  exports: [RouterModule]
})
export class AppRoutingModule { }
