<template>
  <v-container class="ma-2">
    <v-row>
      <h2>{{ item.fullname }}({{ item.code }})</h2>
    </v-row>
    <v-row v-if="!item.accountConfirmed">
      <MessageComponent
        description="This user didn't confirmed his account!"
        fontSize="16px"
        fontWeight="bold"
        :backgroundColor="WARNING_BACKGROUND_AMBER_LIGHTEN_5"
        icon="mdi-information"
        :color="WARNING_AMBER_DARKEN_4"
      />
    </v-row>
    <v-container class="mt-2">
      <p><strong>Email: </strong>{{ item.id }}</p>
      <p><strong>Role: </strong>{{ item.role }}</p>
      <p><strong>Creared on: </strong>{{ item.created }}</p>
      <p><strong>Department: </strong>{{ item.departmentName }}</p>
      <p><strong>{{item.role==Role[2]?'Employment year':'Enrollment year'}}: </strong>{{ item.year }}</p>
      <p><strong>Specialization:</strong></p>
      <ul
        v-for="specializationId in item.specializationIds"
        :key="specializationId"
      >
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
import { WARNING_AMBER_DARKEN_4, WARNING_BACKGROUND_AMBER_LIGHTEN_5 } from "@/shared/constants";
import MessageComponent from "@/components/shared-components/MessageComponent.vue";
import { Role } from "@/shared/enums";

export default Vue.extend({
  name: "UserInfoCardComponent",
  components:{ MessageComponent},
  props: {
    item: {
      type: Object as () => UserViewModule,
      required: true,
    },
  },
  data: function(){
    return {
      Role,
      WARNING_BACKGROUND_AMBER_LIGHTEN_5,
      WARNING_AMBER_DARKEN_4
    };
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