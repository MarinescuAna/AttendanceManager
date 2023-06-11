<template>
  <v-container class="ma-2">
    <v-row>
      <h2>{{ item.fullname }}({{ item.code }})</h2>
    </v-row>
    <v-row v-if="!item.accountConfirmed">
      <v-alert text type="warning"
        >This user didn't confirmed his account!</v-alert
      >
    </v-row>
    <v-container class="mt-2">
      <p><strong>Email: </strong>{{ item.id }}</p>
      <p><strong>Role: </strong>{{ item.role }}</p>
      <p><strong>Department: </strong>{{ item.departmentName }}</p>
      <p><strong>Enrollment year: </strong>{{ item.year }}</p>
      <p><strong>Specialization:</strong></p>
      <ul v-for="specializationId in item.specializationIds" :key="specializationId">
          {{
          getSpecializationName(specializationId)
          }}
      </ul>
    </v-container>
  </v-container>
</template>
  <script lang="ts">
import { UserViewModule } from "@/modules/view-modules";
import storeHelper from "@/store/store-helper";
import Vue from "vue";

export default Vue.extend({
  name: "UserInfoCardComponent",
  props: {
    item: {
      type: Object as () => UserViewModule,
      required: true,
    },
  },
  methods: {
    getSpecializationName: function (id: number): string {
      return storeHelper.specializationStore.specializations.find(
        (s) => s.id === id
      )!.name;
    },
  },
});
</script>