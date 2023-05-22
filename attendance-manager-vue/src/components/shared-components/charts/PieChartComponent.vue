<template>
    <VueApexCharts
      width="1000"
      type="pie"
      class="ma-3"
      :options="chartOptions"
      :series="values"
    ></VueApexCharts>
</template>

<script lang="ts">
import Vue from "vue";
import VueApexCharts from "vue-apexcharts";

export default Vue.extend({
  name: "PieChartComponent",
  components: { VueApexCharts },
  props: {
    /** Data to display in the bar chart*/
    values: {
      type: Array,
      required: true,
      default: () => []
    },
    /** Data to display in the bar chart*/
    labels: {
      type: Array,
      required: true,
      default: () => []
    }
  },
  data: function () {
    return {
      chartOptions: {
        chart: {
          type: "pie"
        },
        labels: this.labels,
        legend: {
        position: 'bottom',
        fontSize: '14px' // Adjust the font size as desired
      }
      }
    };
  },
});
</script>