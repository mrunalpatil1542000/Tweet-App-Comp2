import { Segment, List, Label, Item, Image } from "semantic-ui-react";
import { Link } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { useStore } from "../../../app/stores/store";

export default observer(function TweetDetailedSidebar() {
	const { tweetStore, userStore } = useStore();

	const { selectedTweet } = tweetStore;
	return (
		<>
			<Segment
				textAlign="center"
				style={{ border: "none" }}
				attached="top"
				secondary
				inverted
				color="teal"
			>
				{tweetStore.loadCurrentLikes().length} people liked this tweet
			</Segment>

			{tweetStore.loadCurrentLikes().map((x) => {
				return (
					<Segment key={x.loginId} attached>
						<List relaxed divided>
							<Item style={{ position: "relative" }}>
								{x.email === selectedTweet?.user!.email && (
									<Label
										style={{ position: "absolute" }}
										color="orange"
										ribbon="right"
										content="Host"
									/>
								)}
								<Image size="tiny" src={"/assets/user.png"} />
								<Item.Content verticalAlign="middle">
									<Item.Header as="h3">
										<Link to={`#`}>{x.firstName}</Link>
									</Item.Header>
									<Item.Extra style={{ color: "orange" }}>
										<br />
										<br />
										{x.email}
									</Item.Extra>
								</Item.Content>
							</Item>
							{/* );
						})} */}
						</List>
					</Segment>
				);
			})}
		</>
	);
});
