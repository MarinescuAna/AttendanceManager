<template>
  <div v-if="departments.length != 0">
    <v-data-table
      :headers="headers"
      :items="departments"
      :expanded.sync="expanded"
      item-key="id"
      class="elevation-1"
      show-expand
    >
      <template v-slot:expanded-item="{ headers, item }">
        <td :colspan="headers.length">
          <DepartmentDetailsExpanded :department="item" />
        </td>
      </template>
    </v-data-table>
  </div>
  <div v-else>
    <MessageComponent
      description="There is no department created. Go to the <b>Create department</b> tab and add some departments."
      fontSize="20px"
      fontWeight="bold"
      icon="mdi-information"
      :color="WARNING_AMBER_DARKEN_4"
    />
  </div>
</template>
  
  <script lang="ts">
import Vue from "vue";
import MessageComponent from "../shared-components/MessageComponent.vue";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";
import storeHelper from "@/store/store-helper";
import DepartmentDetailsExpanded from "@/components/department-specialization/DepartmentDetailsExpanded.vue";
import { DepartmentViewModule } from "@/modules/view-modules";

export default Vue.extend({
  name: "DepartmentSpecializationViewTable",
  components: {
    MessageComponent,
    DepartmentDetailsExpanded
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
      expanded: [],
      headers: [
        {
          text: "Departments",
          value: "name",
          class: "text-left black--text text-h6",
        },
      ],
    };
  },
  computed: {
    departments: function (): DepartmentViewModule[] {
      return storeHelper.departmentStore.departments;
    },
  },
});
</script>