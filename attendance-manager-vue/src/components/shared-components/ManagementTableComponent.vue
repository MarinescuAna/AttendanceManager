<template>
  <div v-if="dataSource.length != 0">
    <v-data-table
      :headers="headers"
      :items="dataSource"
      :expanded.sync="expanded"
      item-key="id"
      dense
      :show-expand="expandDetails"
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
  </div>
  <div v-else>
    <span v-html="displayMessage" class="i-message"></span>
  </div>
</template>

<style scoped>
.i-message{
  font-size: 24px;
  font-weight: bold;
}
</style>

<script lang="ts">
import { ManagementDataType } from "@/shared/enums";
import Vue from "vue";
import UserInfoCardComponent from "../user/UserInfoCardComponent.vue";
import DepartmentInfoCardComponent from "../department-specialization/DepartmentInfoCardComponent.vue";
import { TableModule } from "@/modules/shared";
import { HeaderModule } from "./Headers";

export default Vue.extend({
  name: "ManagementTableComponent",
  props: {
    /** Use this to expand details for each row */
    expandDetails: {
      type: Boolean,
      default: true,
    },
    /** The headers list */
    headers: {
      type: Array as () => HeaderModule[],
      required: true,
    },
    /** List with all the data */
    dataSource: {
      type: Array as () => TableModule[],
      required: true,
    },
    /** Title */
    title: {
      type: String,
      required: true,
    },
    /** Data type in case that the exanded value was set on true */
    type: {
      type: () => ManagementDataType,
    },
    /** Error message that needs to be display when the tables has no data to be displayed */
    displayMessage: {
      type: String,
      required: true,
    },
  },
  components: {
    UserInfoCardComponent,
    DepartmentInfoCardComponent,
  },
  data: function () {
    return {
      /** Elements that are currently expanded */
      expanded: [],
    };
  },
});
</script>