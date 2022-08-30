import { Link } from "react-router-dom";
import { Button, Icon, Item, Segment } from "semantic-ui-react";
import { Tweet } from "../../../app/models/Tweet";

interface Props {
	tweet: Tweet;
}
const TweetListItem = ({ tweet }: Props) => {
	return (
		<>
			<Segment.Group>
				<Segment>
					<Item.Group>
						<Item>
							<Item.Image
								floated="left"
								size="tiny"
								circular
								src="/assets/user.png"
							/>
						</Item>
						<Item.Content>
							<Item.Header># {tweet.tag}</Item.Header>
							<Item.Description>
								By @{tweet.user!.firstName}.{tweet.user!.lastName}
							</Item.Description>
						</Item.Content>
					</Item.Group>
				</Segment>
				<Segment>
					<span>
						<Icon name="clock" />
						{tweet.datePosted}
					</span>
				</Segment>
				<Segment clearing>
					<span>{tweet.subject}</span>
					<Button
						floated="right"
						as={Link}
						to={`/details/${tweet.id}`}
						icon="info"
						color="blue"
					/>
				</Segment>
			</Segment.Group>
		</>
	);
};

export default TweetListItem;
