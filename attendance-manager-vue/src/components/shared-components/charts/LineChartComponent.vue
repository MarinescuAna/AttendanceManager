<template>
  <VueApexCharts
    width="1000"
    type="line"
    class="ma-3"
    :options="chartOptions"
    :series="values"
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
    values: {
      type: Array,
      required: true,
      default: () => [],
    },
    /** Data to display in the bar chart*/
    labels: {
      type: Array,
      required: true,
      default: () => [],
    },
  },
  data: function () {
    return {
      chartOptions: {
        chart: {
          type: "line",
        },
        stroke: {
          curve: "straight",
        },
        dataLabels: {
          enabled: true,
        },
        xaxis: {
          categories: this.labels,
        },
      },
    };
  },
});
</script>