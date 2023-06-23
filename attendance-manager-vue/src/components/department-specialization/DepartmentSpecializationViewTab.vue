<template>
  <div v-if="departments.length != 0">
    <v-data-table
      :headers="headers"
      :items="departments"
      :expanded.sync="expanded"
      sort-by="name"
      item-key="id"
      class="elevation-1"
      show-expand
    >
      <template v-slot:[`item.updatedOn`]="{ item }">
        {{ getRelativeTime(item.updatedOn) }}
      </template>
      <template v-slot:[`item.actions`]="{ item }">
        <v-icon
          color="black"
          @click="onDeleteDepartment(item.id)"
          v-if="item.linkedSpecializations == 0"
        >
          mdi-delete {{ item.id }}
        </v-icon>
        <v-tooltip bottom v-else>
          <template v-slot:activator="{ on, attrs }">
            <v-icon color="black" v-bind="attrs" v-on="on">
              mdi-information-outline
            </v-icon>
          </template>
          <span>You can't delete this department because you have some link specializations to it!</span>
        </v-tooltip>
      </template>
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
import moment from "moment";
import { Toastification } from "@/plugins/vue-toastification";

export default Vue.extend({
  name: "DepartmentSpecializationViewTable",
  components: {
    MessageComponent,
    DepartmentDetailsExpanded,
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
        {
          text: "Linked specializations",
          value: "linkedSpecializations",
          class: "text-left black--text text-h6",
        },
        {
          text: "Last update",
          value: "updatedOn",
          class: "text-left black--text text-h6",
        },
        {
          text: "",
          value: "actions",
          sortable: false,
        },
      ],
    };
  },
  computed: {
    departments: function (): DepartmentViewModule[] {
      return storeHelper.departmentStore.departments;
    },
  },
  methods: {
    onDeleteDepartment: async function (id: number): Promise<void> {
      const result = await storeHelper.departmentStore.deleteDepartment(id);

      if (result) {
        Toastification.success("The department was successfully deleted!");
      }
    },
    getRelativeTime(updateOn: string) {
      return moment(new Date(updateOn)).fromNow();
    },
  },
});
</script>