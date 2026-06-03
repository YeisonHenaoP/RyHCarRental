import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ApiService } from './core/services/api.service';
import { Customer } from './core/models/customer.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <h1>RyH Car Rental</h1>
    <p>Conexión a la API: {{ connectionStatus }}</p>
    @if (customers.length > 0) {
      <ul>
        @for (customer of customers; track customer.id) {
          <li>{{ customer.fullName }} - {{ customer.email }}</li>
        }
      </ul>
    }
    <router-outlet />
  `
})
export class App implements OnInit {
  private apiService = inject(ApiService);
  private cdr = inject(ChangeDetectorRef);
  connectionStatus = 'Verificando...';
  customers: Customer[] = [];

  ngOnInit(): void {
    this.apiService.get<Customer[]>('Customers').subscribe({
      next: (customers) => {
        this.connectionStatus = '✔ Conectado exitosamente';
        this.customers = customers;
        this.cdr.detectChanges();
        console.log('Customers:', this.customers);
      },
      error: (err) => {
        this.connectionStatus = '✘ Error de conexión';
        this.cdr.detectChanges();
        console.error('Error:', err);
      }
    });
  }
}