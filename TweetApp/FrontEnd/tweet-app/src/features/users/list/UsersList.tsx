import { observer } from "mobx-react-lite";
import { Item, Segment } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";

const UsersList = () => {
	const { userStore } = useStore();
	const { users } = userStore;
	return (
		<Segment>
			<Item.Group divided>
				{users &&
					users.map((user) => {
						return (
							<Item key={user.loginId}>
								<Item.Content>
									<Item.Header>
										{user.firstName} {user.lastName}
									</Item.Header>
									<Item.Meta>Username: {user.email}</Item.Meta>
									<Item.Description>
										Contact:+91{user.contactNumber}
									</Item.Description>
								</Item.Content>
							</Item>
						);
					})}
			</Item.Group>
		</Segment>
	);
};

export default observer(UsersList);
