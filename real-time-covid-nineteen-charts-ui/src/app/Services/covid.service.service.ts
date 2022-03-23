import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { CovidModel } from '../Models/covid.model';

@Injectable({
  providedIn: 'root',
})
export class CovidService {
  private path: string = 'https://localhost:7103/CovidHub';
  covidChartList: any[] = [];
  private hubConnection?: signalR.HubConnection;

  constructor() {}

  private startInvoke(): void {
    this.hubConnection?.invoke('GetCovidListAsync').catch((err) => {
      throw new Error(err);
    });
  }

  startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.path, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .build();
    this.hubConnection
      .start()
      .then(() => {
        this.startInvoke();
      })
      .catch((error) => {
        throw new Error(error);
      });
  }

  startListener() {
    this.hubConnection?.on('ReceiveCovidList', (covidCharts: CovidModel[]) => {
      this.covidChartList = [];
      covidCharts.forEach((item:CovidModel) => {
        this.covidChartList.push([item.covidDate, , item.counts[1], item.counts[2], item.counts[3], item.counts[4]])
      });
    });
  }
}
