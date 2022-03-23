import { Component, OnInit } from '@angular/core';
import { ChartType } from 'angular-google-charts';
import { CovidService } from './Services/covid.service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Gerçek Zamanlı Covid-19 Uygulaması | SignalR - Angular';
  public covidChartList: any[] = [];
  columnNames: string[] = [
    'Tarih',
    'İstanbul',
    'Ankara',
    'İzmir',
    'Bursa',
    'Manisa',
  ];

  options: any = {
    legend: {
      position: 'Bottom',
    },
  };

  chartType: ChartType = ChartType.LineChart;

  constructor(public covidService: CovidService) {}

  ngOnInit(): void {
    this.covidService.startConnection();
    this.covidService.startListener();
  }
}
