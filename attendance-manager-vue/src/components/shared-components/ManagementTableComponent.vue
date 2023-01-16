<template>
  <v-data-table
    :headers="headers"
    :items="dataSource"
    :expanded.sync="expanded"
    item-key="id"
    dense
    show-expand
    class="elevation-1"
  >
    <template v-slot:top>
      <v-toolbar flat>
        <v-toolbar-title>{{ title }}</v-toolbar-title>
      </v-toolbar>
    </template>
    <template v-slot:expanded-item="{ headers, item }">
      <td :colspan="headers.length">
        <user-info-card-component
          :item="item"
          v-if="type == 0"
        ></user-info-card-component>
        <department-info-card-component
          :item="item"
          v-if="type == 1"
        ></department-info-card-component>
      </td>
    </template>
  </v-data-table>
</template>

<script lang="ts">
import { ManagementDataType } from "@/shared/enums";
import Vue from "vue";
import UserInfoCardComponent from "../user/UserInfoCardComponent.vue";
import DepartmentInfoCardComponent from "../department-specialization/DepartmentInfoCardComponent.vue";
import { TableModule } from "@/modules/shared";
import { HeaderModule } from "./Headers";

export default Vue.extend({
  props: {
    // The headers list
    headers: Array as () => HeaderModule[],
    // List with all the data
    dataSource: Array as () => TableModule[],
    // Title
    title: String,
    // Data type
    type: () => ManagementDataType,
  },
  components: {
    UserInfoCardComponent,
    DepartmentInfoCardComponent,
  },
  data() {
    return {
      // Elements that are currently expanded
      expanded: [],
    };
  },
});
</script>