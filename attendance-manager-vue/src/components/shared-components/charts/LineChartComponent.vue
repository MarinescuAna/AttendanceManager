<template>
  <VueApexCharts
    width="1000"
    type="line"
    class="ma-3"
    :options="chartOptions"
    :series="chartData"
  ></VueApexCharts>
</template>
  
  <script lang="ts">
import Vue from "vue";
import VueApexCharts from "vue-apexcharts";

export default Vue.extend({
  name: "LineChartComponent",
  components: { VueApexCharts },
  props: {
    /** Data to display in the bar chart*/
    chartData: {
      type: Array,
      required: true,
      default: () => []
    },
    /** Data to display in the bar chart*/
    xAxiesLabels: {
      type: Array,
      required: true,
      default: () => []
    },
    /** The title of the bar chart */
    title: {
      type: String,
      required: true,
    },
  },
  data: function () {
    return {
      chartOptions: {
        chart: {
          height: 350,
          type: "line",
        },
        stroke: {
          width: [0, 4],
        },
        title: {
          text: this.title,
        },
        dataLabels: {
          enabled: true,
          enabledOnSeries: [1],
        },
        labels: this.xAxiesLabels,
        xaxis: {
          type: "datetime",
        },
      },
    };
  },
});
</script>